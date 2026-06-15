using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos360.Data;
using Pedidos360.ViewModels;

namespace Pedidos360.Controllers;

public class HomeController : Controller
{
    private readonly Pedidos360Context _context;

    public HomeController(Pedidos360Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeDashboardViewModel
        {
            TotalCategorias  = await _context.Categorias.CountAsync(),
            TotalProductos   = await _context.Productos.CountAsync(),
            TotalClientes    = await _context.Clientes.CountAsync(),
            ProductosActivos = await _context.Productos.CountAsync(p => p.Activo)
        };

        return View(viewModel);
    }

    public IActionResult Error()
    {
        return View(new Pedidos360.Models.ErrorViewModel
        {
            RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
