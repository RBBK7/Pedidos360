using Microsoft.AspNetCore.Mvc.Rendering;
using Pedidos360.Models;
using System.ComponentModel.DataAnnotations;
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

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "El correo no es valido")]
    public string Correo { get; set; } = null!;

    [Required(ErrorMessage = "El telefono es obligatorio")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El telefono debe tener 8 dígitos")]
    public string Telefono { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una provincia")]
    public int ProvinciaId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cantón")]
    public int CantonId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un distrito")]
    public int DistritoId { get; set; }

    [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
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

// Linea enviada desde el carrito hacia el servidor para calcular y confirmar
public class LineaCalculoDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Producto invalido")]
    public int ProductoId { get; set; }

    [Range(1, 1000, ErrorMessage = "La cantidad debe estar entre 1 y 1000")]
    public int Cantidad { get; set; }

    [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0% y 100%")]
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