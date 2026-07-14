using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using Pedidos360.Data;

using Pedidos360.ViewModels;


namespace Pedidos360.Controllers;


public class HomeController : Controller

{

    private readonly Pedidos360Context _context;

    private readonly ILogger<HomeController> _logger;


    public HomeController(

        Pedidos360Context context,

        ILogger<HomeController> logger)

    {

        _context = context;

        _logger = logger;

    }


    public async Task<IActionResult> Index()

    {

        _logger.LogInformation("Ingreso al Dashboard principal");


        var estadoActivoId = await _context.Estados

            .Where(e => e.Descripcion == "Activo")

            .Select(e => e.Id)

            .FirstOrDefaultAsync();


        var viewModel = new HomeDashboardViewModel

        {

            TotalCategorias = await _context.Categorias.CountAsync(),

            TotalProductos = await _context.Productos.CountAsync(),

            TotalClientes = await _context.Clientes.CountAsync(),

            ProductosActivos = await _context.Productos.CountAsync(p => p.EstadoId == estadoActivoId),

            TotalPedidos = await _context.Pedidos.CountAsync()

        };


        return View(viewModel);

    }


    public IActionResult Error()

    {

        _logger.LogError("Se produjo un error en la aplicación");


        return View(new Pedidos360.Models.ErrorViewModel

        {

            RequestId = System.Diagnostics.Activity.Current?.Id

                       ?? HttpContext.TraceIdentifier

        });

    }


    public IActionResult StatusCode()

    {

        if (HttpContext.Response.StatusCode == 404)

        {

            return View("Error404");

        }


        return View("Error");

    }

}