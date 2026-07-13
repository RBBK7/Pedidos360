using Microsoft.AspNetCore.Mvc.Rendering;
using Pedidos360.Models;
using X.PagedList;

namespace Pedidos360.ViewModels;


// ViewModel para Create/Edit de Producto 
public class ProductoFormViewModel
{
    public Producto Producto { get; set; } = new();

    public List<SelectListItem> Categorias { get; set; } = [];

    public List<SelectListItem> Estados { get; set; } = [];
}

// ViewModel para el Index de Productos (lista paginada + filtros)
public class ProductoIndexViewModel
{
    public IPagedList<Producto> Productos { get; set; } = null!;

    // Filtros actuales (para mantener valores en el form)
    public string? NombreFiltro { get; set; }
    public int? CategoriaFiltro { get; set; }

    public List<SelectListItem> Categorias { get; set; } = [];
}

// ── Cliente

public class ClienteFormViewModel
{
    public string? CedulaOriginal { get; set; }

    public Cliente Cliente { get; set; } = new();

    public string Correo { get; set; } = null!;
    public string Telefono { get; set; } = null!;

    public int ProvinciaId { get; set; }
    public int CantonId { get; set; }
    public int DistritoId { get; set; }
    public string? OtrasSenas { get; set; }

    public List<SelectListItem> Provincias { get; set; } = [];
    public List<SelectListItem> Cantones { get; set; } = [];
    public List<SelectListItem> Distritos { get; set; } = [];
}

public class ClienteIndexViewModel
{
    public IPagedList<ClienteListItem> Clientes { get; set; } = null!;

    public string? BusquedaFiltro { get; set; }
}

public class ClienteListItem
{
    public string Cedula { get; set; } = null!;
    public string NombreCompleto { get; set; } = null!;
    public string? Correo { get; set; }
    public string? Telefono { get; set; }
}



// ViewModel para la pantalla de creación de pedidos (cliente + carrito armado por AJAX)
public class PedidoCreateViewModel
{
    public List<SelectListItem> Clientes { get; set; } = [];
}

// ViewModel para el Index de Pedidos (lista paginada)
public class PedidoIndexViewModel
{
    public IPagedList<Pedido> Pedidos { get; set; } = null!;

    public List<SelectListItem> EstadosPedido { get; set; } = [];
}

// Resultado del autosuggest de productos (GET /Pedidos/BuscarProductos)
public class ProductoBusquedaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public decimal ImpuestoPorc { get; set; }
    public int Stock { get; set; }
}

// Linea enviada desde el carrito hacia el servidor para calculary confirmar
public class LineaCalculoDto
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal DescuentoPorc { get; set; }
}

// Linea ya calculada por el servidor devuelta al cliente
public class LineaCalculadaDto
{
    public int ProductoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal DescuentoPorc { get; set; }
    public decimal ImpuestoPorc { get; set; }
    public decimal TotalLinea { get; set; }
    public int StockDisponible { get; set; }
}

// Respuesta de POST /Pedidos/Calcular
public class TotalesCalculadosDto
{
    public decimal Subtotal { get; set; }
    public decimal Descuento { get; set; }
    public decimal Impuesto { get; set; }
    public decimal Total { get; set; }
    public List<LineaCalculadaDto> Lineas { get; set; } = [];
}

// Cuerpo de POST /Pedidos/Confirmar
public class ConfirmarPedidoDto
{
    public string ClienteId { get; set; } = string.Empty;
    public List<LineaCalculoDto> Lineas { get; set; } = [];
}

// Dashboard 

public class HomeDashboardViewModel
{
    public int TotalCategorias { get; set; }
    public int TotalProductos { get; set; }
    public int TotalClientes { get; set; }
    public int ProductosActivos { get; set; }
    public int TotalPedidos { get; set; }
}