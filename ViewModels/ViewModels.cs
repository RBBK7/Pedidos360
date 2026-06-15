using Microsoft.AspNetCore.Mvc.Rendering;
using Pedidos360.Models;
using X.PagedList;

namespace Pedidos360.ViewModels;

// ── Producto ─

/// ViewModel para Create/Edit de Producto 
public class ProductoFormViewModel
{
    public Producto Producto { get; set; } = new();

    public List<SelectListItem> Categorias { get; set; } = [];
}

/// ViewModel para el Index de Productos (lista paginada + filtros)
public class ProductoIndexViewModel
{
    public IPagedList<Producto> Productos { get; set; } = null!;

    // Filtros actuales (para mantener valores en el form)
    public string? NombreFiltro { get; set; }
    public int? CategoriaFiltro { get; set; }

    public List<SelectListItem> Categorias { get; set; } = [];
}

// ── Cliente

///ViewModel para el Index de Clientes (lista paginada + busqueda)
public class ClienteIndexViewModel
{
    public IPagedList<Cliente> Clientes { get; set; } = null!;

    public string? BusquedaFiltro { get; set; }
}

// ── Dashboard 

public class HomeDashboardViewModel
{
    public int TotalCategorias { get; set; }
    public int TotalProductos { get; set; }
    public int TotalClientes { get; set; }
    public int ProductosActivos { get; set; }
}
