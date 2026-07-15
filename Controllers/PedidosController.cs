using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pedidos360.Data;
using Pedidos360.Models;
using Pedidos360.Services;
using Pedidos360.ViewModels;
using X.PagedList.Extensions;

namespace Pedidos360.Controllers;

[Authorize]
public class PedidosController : Controller
{
    private readonly Pedidos360Context _context;
    private const int PageSize = 10;

    public PedidosController(Pedidos360Context context)
    {
        _context = context;
    }

    // GET: /Pedidos

    [Authorize(Roles = "Admin,Ventas,Operaciones")]
    public async Task<IActionResult> Index(int page = 1)
    {
        var lista = await _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Estado)
            .OrderByDescending(p => p.Id)
            .ToListAsync();

        var viewModel = new PedidoIndexViewModel
        {
            Pedidos = lista.ToPagedList(page, PageSize),
            EstadosPedido = await BuildEstadosPedidoSelectAsync()
        };

        return View(viewModel);
    }

    // GET: /Pedidos/Details/
    [Authorize(Roles = "Admin,Ventas,Operaciones")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();

        var pedido = await _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Estado)
            .Include(p => p.Detalles).ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pedido is null) return NotFound();

        ViewBag.EstadosPedido = await BuildEstadosPedidoSelectAsync();

        return View(pedido);
    }

    // POST: /Pedidos/CambiarEstado
    // Actualiza el estado de un pedido desde el menu desplegable 

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstado(int id, int estadoId)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido is null)
            return NotFound(new { message = "El pedido no existe." });

        var estado = await _context.Estados.FindAsync(estadoId);
        if (estado is null)
            return BadRequest(new { message = "El estado seleccionado no es válido." });

        pedido.EstadoId = estadoId;
        await _context.SaveChangesAsync();

        return Json(new { success = true, estadoId = estado.Id, estadoDescripcion = estado.Descripcion });
    }

    // GET: /Pedidos/ExportarPdf/
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ExportarPdf(int id)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Estado)
            .Include(p => p.Detalles).ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pedido is null) return NotFound();

        var pdfBytes = PedidoPdfService.Generar(pedido);
        return File(pdfBytes, "application/pdf", $"Pedido_{pedido.Id}.pdf");
    }

    // GET: /Pedidos/Create
    [Authorize(Roles = "Admin,Ventas")]
    public async Task<IActionResult> Create()
    {
        var viewModel = new PedidoCreateViewModel
        {
            Clientes = await _context.Clientes
                .OrderBy(c => c.Nombre)
                .Select(c => new SelectListItem
                {
                    Value = c.Cedula,
                    Text = (c.Nombre + " " + c.ApellidoPaterno + " — " + c.Cedula)
                })
                .ToListAsync()
        };

        return View(viewModel);
    }

    // GET: /Pedidos/BuscarProductos
    // Autosuggest AJAX de productos activos por nombre 
    [Authorize(Roles = "Admin,Ventas,Operaciones")]
    [HttpGet]
    public async Task<IActionResult> BuscarProductos(string? q)
    {
        var query = _context.Productos.Where(p => p.Estado != null && p.Estado.Descripcion == "Activo").AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(p => p.Nombre.Contains(q));

        var resultados = await query
            .OrderBy(p => p.Nombre)
            .Take(10)
            .Select(p => new ProductoBusquedaDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                ImpuestoPorc = p.ImpuestoPorc,
                Stock = p.Stock
            })
            .ToListAsync();

        return Json(resultados);
    }

    // POST: /Pedidos/Calcular
    // Recalcula subtotal,descuento,impuesto y total en vivo, sin persistir nada
    [Authorize(Roles = "Admin,Ventas")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Calcular([FromBody] List<LineaCalculoDto>? lineas)
    {
        var resultado = await CalcularTotalesAsync(lineas);
        return Json(resultado);
    }

    // POST: /Pedidos/Confirmar
    // Valida stock, crea el pedido + detalle y descuenta el inventario
    [Authorize(Roles = "Admin,Ventas")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Confirmar([FromBody] ConfirmarPedidoDto? data)
    {
        if (data is null || string.IsNullOrWhiteSpace(data.ClienteId) || data.Lineas is null || data.Lineas.Count == 0)
            return BadRequest(new { message = "Seleccione un cliente y agregue al menos un producto." });

        var cliente = await _context.Clientes.FindAsync(data.ClienteId);
        if (cliente is null)
            return BadRequest(new { message = "El cliente seleccionado no existe." });

        var totales = await CalcularTotalesAsync(data.Lineas);

        if (totales.Lineas.Count == 0)
            return BadRequest(new { message = "No se encontraron productos válidos en el pedido." });

        var sinStock = totales.Lineas.Where(l => l.Cantidad > l.StockDisponible).ToList();
        if (sinStock.Count > 0)
            return BadRequest(new
            {
                message = "Stock insuficiente para: " + string.Join(", ", sinStock.Select(l => l.Nombre)) + "."
            });

        using var transaction = await _context.Database.BeginTransactionAsync();

        var estadoPendiente = await ObtenerOCrearEstadoAsync("Pendiente");

        var pedido = new Pedido
        {
            ClienteId = data.ClienteId,
            EstadoId = estadoPendiente.Id,
            RolId = 1, // TODO: reemplazar por el rol real cuando se maneje en la app
            Fecha = DateTime.Now,
            Subtotal = totales.Subtotal,
            Impuesto = totales.Impuesto,
            Descuento = totales.Descuento,
            Total = totales.Total
        };

        foreach (var linea in totales.Lineas)
        {
            var producto = await _context.Productos.FindAsync(linea.ProductoId);
            if (producto is null) continue;

            // Re-validar stock dentro de la transacción por si cambió mientras se armaba el pedido
            if (producto.Stock < linea.Cantidad)
                return BadRequest(new { message = $"Stock insuficiente para \"{producto.Nombre}\"." });

            producto.Stock -= linea.Cantidad;

            pedido.Detalles.Add(new DetallePedido
            {
                ProductoId = linea.ProductoId,
                Cantidad = linea.Cantidad,
                PrecioUnitario = linea.PrecioUnitario,
                DescuentoPorc = linea.DescuentoPorc,
                ImpuestoPorc = linea.ImpuestoPorc,
                TotalLinea = linea.TotalLinea
            });
        }

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        TempData["SuccessMessage"] = $"Pedido #{pedido.Id} creado correctamente.";
        return Json(new { success = true, redirectUrl = Url.Action(nameof(Details), new { id = pedido.Id }) });
    }

    // ── Helpers

    private async Task<TotalesCalculadosDto> CalcularTotalesAsync(List<LineaCalculoDto>? lineas)
    {
        var resultado = new TotalesCalculadosDto();
        if (lineas is null || lineas.Count == 0) return resultado;

        var productoIds = lineas.Select(l => l.ProductoId).Distinct().ToList();
        var productos = await _context.Productos
            .Where(p => productoIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        foreach (var linea in lineas)
        {
            if (!productos.TryGetValue(linea.ProductoId, out var producto)) continue;

            var cantidad = Math.Max(1, linea.Cantidad);
            var descuentoPorc = Math.Clamp(linea.DescuentoPorc, 0, 100);

            var subtotalLinea = cantidad * producto.Precio;
            var descuentoLinea = subtotalLinea * (descuentoPorc / 100m);
            var baseLinea = subtotalLinea - descuentoLinea;
            var impuestoLinea = baseLinea * (producto.ImpuestoPorc / 100m);
            var totalLinea = baseLinea + impuestoLinea;

            resultado.Subtotal += subtotalLinea;
            resultado.Descuento += descuentoLinea;
            resultado.Impuesto += impuestoLinea;
            resultado.Total += totalLinea;

            resultado.Lineas.Add(new LineaCalculadaDto
            {
                ProductoId = producto.Id,
                Nombre = producto.Nombre,
                Cantidad = cantidad,
                PrecioUnitario = producto.Precio,
                DescuentoPorc = descuentoPorc,
                ImpuestoPorc = producto.ImpuestoPorc,
                TotalLinea = Math.Round(totalLinea, 2),
                StockDisponible = producto.Stock
            });
        }

        resultado.Subtotal = Math.Round(resultado.Subtotal, 2);
        resultado.Descuento = Math.Round(resultado.Descuento, 2);
        resultado.Impuesto = Math.Round(resultado.Impuesto, 2);
        resultado.Total = Math.Round(resultado.Total, 2);

        return resultado;
    }

    // Si el valor todavía no existe se crea la primera vez que se usa.
    private async Task<Estado> ObtenerOCrearEstadoAsync(string descripcion)
    {
        var estado = await _context.Estados.FirstOrDefaultAsync(e => e.Descripcion == descripcion);
        if (estado is null)
        {
            estado = new Estado { Descripcion = descripcion };
            _context.Estados.Add(estado);
            await _context.SaveChangesAsync();
        }
        return estado;
    }

    // Estados posibles para el flujo de un pedido, en orden lógico.
    private static readonly string[] EstadosPedidoPosibles =
        ["Pendiente", "Confirmado", "Enviado", "Entregado", "Cancelado"];

    // Construye el <select> con los estados válidos para un pedido (independiente de
    // los estados "Activo"/"Inactivo" que usan Productos y Clientes).
    private async Task<List<SelectListItem>> BuildEstadosPedidoSelectAsync()
    {
        var lista = new List<SelectListItem>();
        foreach (var descripcion in EstadosPedidoPosibles)
        {
            var estado = await ObtenerOCrearEstadoAsync(descripcion);
            lista.Add(new SelectListItem { Value = estado.Id.ToString(), Text = estado.Descripcion });
        }
        return lista;
    }
}