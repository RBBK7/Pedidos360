using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pedidos360.Data;
using Pedidos360.Models;
using Pedidos360.ViewModels;
using X.PagedList.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Pedidos360.Controllers;

[Authorize]
public class ClientesController : Controller
{
    private readonly Pedidos360Context _context;
    private const int PageSize = 10;

    public ClientesController(Pedidos360Context context)
    {
        _context = context;
    }


    [Authorize(Roles = "Admin,Ventas")]
    public async Task<IActionResult> Index(string? busqueda, int page = 1)
    {
        var query = _context.Clientes
            .Include(c => c.Correos)
            .Include(c => c.Telefonos)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(busqueda))
            query = query.Where(c =>
                c.Nombre.Contains(busqueda) ||
                c.Cedula.Contains(busqueda));

        query = query.OrderBy(c => c.Nombre);

        var lista = await query.Select(c => new ClienteListItem
        {
            Cedula = c.Cedula,
            NombreCompleto = c.Nombre + " " + c.ApellidoPaterno + " " + c.ApellidoMaterno,
            Correo = c.Correos.Select(x => x.Correo).FirstOrDefault(),
            Telefono = c.Telefonos.Select(x => x.Telefono).FirstOrDefault()
        }).ToListAsync();

        var viewModel = new ClienteIndexViewModel
        {
            Clientes = lista.ToPagedList(page, PageSize),
            BusquedaFiltro = busqueda
        };

        return View(viewModel);
    }

    [Authorize(Roles = "Admin,Ventas")]
    public async Task<IActionResult> Details(string? id)
    {
        if (id is null) return NotFound();

        var cliente = await _context.Clientes
            .Include(c => c.Estado)
            .Include(c => c.Correos)
            .Include(c => c.Telefonos)
            .Include(c => c.Direcciones).ThenInclude(d => d.Provincia)
            .Include(c => c.Direcciones).ThenInclude(d => d.Canton)
            .Include(c => c.Direcciones).ThenInclude(d => d.Distrito)
            .FirstOrDefaultAsync(c => c.Cedula == id);

        if (cliente is null) return NotFound();

        return View(cliente);
    }


    [Authorize(Roles = "Admin,Ventas")]
    public async Task<IActionResult> Create()
    {
        return View(await BuildFormViewModelAsync(new ClienteFormViewModel()));
    }

    [Authorize(Roles = "Admin,Ventas")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClienteFormViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(await BuildFormViewModelAsync(viewModel));

        if (await _context.Clientes.AnyAsync(c => c.Cedula == viewModel.Cliente.Cedula))
        {
            ModelState.AddModelError("Cliente.Cedula", "Ya existe un cliente con esa cédula.");
            return View(await BuildFormViewModelAsync(viewModel));
        }

        var cliente = viewModel.Cliente;
        cliente.EstadoId = await ObtenerEstadoActivoIdAsync();
        cliente.Correos = [new CorreoCliente { Cedula = cliente.Cedula, Correo = viewModel.Correo }];
        cliente.Telefonos = [new TelefonoCliente { Cedula = cliente.Cedula, Telefono = viewModel.Telefono }];
        cliente.Direcciones =
        [
            new Direccion
            {
                Cedula      = cliente.Cedula,
                ProvinciaId = viewModel.ProvinciaId,
                CantonId    = viewModel.CantonId,
                DistritoId  = viewModel.DistritoId,
                OtrasSenas  = viewModel.OtrasSenas
            }
        ];

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Cliente \"{cliente.Nombre}\" creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin,Ventas")]
    public async Task<IActionResult> Edit(string? id)
    {
        if (id is null) return NotFound();

        var cliente = await _context.Clientes
            .Include(c => c.Correos)
            .Include(c => c.Telefonos)
            .Include(c => c.Direcciones)
            .FirstOrDefaultAsync(c => c.Cedula == id);

        if (cliente is null) return NotFound();

        var viewModel = new ClienteFormViewModel
        {
            CedulaOriginal = cliente.Cedula,
            Cliente = cliente,
            Correo = cliente.Correos.Select(c => c.Correo).FirstOrDefault() ?? string.Empty,
            Telefono = cliente.Telefonos.Select(t => t.Telefono).FirstOrDefault() ?? string.Empty,
            ProvinciaId = cliente.Direcciones.Select(d => d.ProvinciaId).FirstOrDefault(),
            CantonId = cliente.Direcciones.Select(d => d.CantonId).FirstOrDefault(),
            DistritoId = cliente.Direcciones.Select(d => d.DistritoId).FirstOrDefault(),
            OtrasSenas = cliente.Direcciones.Select(d => d.OtrasSenas).FirstOrDefault()
        };

        return View(await BuildFormViewModelAsync(viewModel));
    }

    [Authorize(Roles = "Admin,Ventas")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, ClienteFormViewModel viewModel)
    {
        if (id != viewModel.CedulaOriginal) return NotFound();

        if (!ModelState.IsValid)
            return View(await BuildFormViewModelAsync(viewModel));

        var clienteDb = await _context.Clientes
            .Include(c => c.Correos)
            .Include(c => c.Telefonos)
            .Include(c => c.Direcciones)
            .FirstOrDefaultAsync(c => c.Cedula == id);

        if (clienteDb is null) return NotFound();

        clienteDb.Nombre = viewModel.Cliente.Nombre;
        clienteDb.ApellidoPaterno = viewModel.Cliente.ApellidoPaterno;
        clienteDb.ApellidoMaterno = viewModel.Cliente.ApellidoMaterno;

        _context.CorreosClientes.RemoveRange(clienteDb.Correos);
        _context.TelefonosClientes.RemoveRange(clienteDb.Telefonos);
        _context.Direcciones.RemoveRange(clienteDb.Direcciones);

        clienteDb.Correos = [new CorreoCliente { Cedula = id, Correo = viewModel.Correo }];
        clienteDb.Telefonos = [new TelefonoCliente { Cedula = id, Telefono = viewModel.Telefono }];
        clienteDb.Direcciones =
        [
            new Direccion
            {
                Cedula      = id,
                ProvinciaId = viewModel.ProvinciaId,
                CantonId    = viewModel.CantonId,
                DistritoId  = viewModel.DistritoId,
                OtrasSenas  = viewModel.OtrasSenas
            }
        ];

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Cliente \"{clienteDb.Nombre}\" actualizado correctamente";
        return RedirectToAction(nameof(Index));
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string? id)
    {
        if (id is null) return NotFound();

        var cliente = await _context.Clientes
            .Include(c => c.Correos)
            .Include(c => c.Telefonos)
            .Include(c => c.Direcciones)
            .Include(c => c.Pedidos)
            .FirstOrDefaultAsync(c => c.Cedula == id);

        if (cliente is null) return NotFound();

        return View(cliente);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var cliente = await _context.Clientes
            .Include(c => c.Correos)
            .Include(c => c.Telefonos)
            .Include(c => c.Direcciones)
            .Include(c => c.Pedidos)
            .FirstOrDefaultAsync(c => c.Cedula == id);

        if (cliente is null) return RedirectToAction(nameof(Index));

        if (cliente.Pedidos.Count > 0)
        {
            TempData["ErrorMessage"] =
                $"No se puede eliminar a \"{cliente.Nombre}\" porque tiene pedidos registrados.";
            return RedirectToAction(nameof(Index));
        }

        _context.CorreosClientes.RemoveRange(cliente.Correos);
        _context.TelefonosClientes.RemoveRange(cliente.Telefonos);
        _context.Direcciones.RemoveRange(cliente.Direcciones);
        _context.Clientes.Remove(cliente);

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Cliente \"{cliente.Nombre}\" eliminado correctamente";
        return RedirectToAction(nameof(Index));
    }

    // ── Helpers

    private async Task<ClienteFormViewModel> BuildFormViewModelAsync(ClienteFormViewModel viewModel)
    {
        viewModel.Provincias = await _context.Provincias
            .OrderBy(p => p.NombreProvincia)
            .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.NombreProvincia })
            .ToListAsync();

        viewModel.Cantones = await _context.Cantones
            .Where(c => c.ProvinciaId == viewModel.ProvinciaId)
            .OrderBy(c => c.NombreCanton)
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.NombreCanton })
            .ToListAsync();

        viewModel.Distritos = await _context.Distritos
            .Where(d => d.CantonId == viewModel.CantonId)
            .OrderBy(d => d.NombreDistrito)
            .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.NombreDistrito })
            .ToListAsync();

        return viewModel;
    }

  //clientes nuevos se marcan como "Activo" automáticamente.
    private async Task<int> ObtenerEstadoActivoIdAsync()
    {
        var estado = await _context.Estados.FirstOrDefaultAsync(e => e.Descripcion == "Activo");
        if (estado is null)
        {
            estado = new Estado { Descripcion = "Activo" };
            _context.Estados.Add(estado);
            await _context.SaveChangesAsync();
        }
        return estado.Id;
    }
}