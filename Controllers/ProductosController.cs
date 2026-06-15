using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pedidos360.Data;
using Pedidos360.Models;
using Pedidos360.ViewModels;
using X.PagedList.Extensions;

namespace Pedidos360.Controllers;

public class ProductosController : Controller
{
    private readonly Pedidos360Context _context;
    private readonly IWebHostEnvironment _env;
    private const int PageSize = 5;

    public ProductosController(Pedidos360Context context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }


    public async Task<IActionResult> Index(string? nombre, int? categoriaId, int page = 1)
    {
        var query = _context.Productos
            .Include(p => p.Categoria)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(nombre))
            query = query.Where(p => p.Nombre.Contains(nombre));

        if (categoriaId.HasValue)
            query = query.Where(p => p.CategoriaId == categoriaId.Value);

        query = query.OrderBy(p => p.Nombre);

        var viewModel = new ProductoIndexViewModel
        {
            Productos      = query.ToPagedList(page, PageSize),
            NombreFiltro   = nombre,
            CategoriaFiltro= categoriaId,
            Categorias     = await BuildCategoriasSelectAsync()
        };

        return View(viewModel);
    }


    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();

        var producto = await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (producto is null) return NotFound();

        return View(producto);
    }


    public async Task<IActionResult> Create()
    {
        return View(await BuildFormViewModelAsync(new Producto { Activo = true }));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductoFormViewModel viewModel, IFormFile? imagenFile)
    {
        // Validar que existan categorias
        if (!await _context.Categorias.AnyAsync())
            ModelState.AddModelError("", "Primero crea al menos una categoría.");

        // Procesar imagen subida
        if (imagenFile is not null && imagenFile.Length > 0)
        {
            viewModel.Producto.ImagenUrl = await GuardarImagenAsync(imagenFile);
            ModelState.Remove("Producto.ImagenUrl");
        }
        else if (string.IsNullOrWhiteSpace(viewModel.Producto.ImagenUrl))
        {
            ModelState.AddModelError("Producto.ImagenUrl", "La imagen es obligatoria al crear el producto.");
        }

        if (!ModelState.IsValid)
            return View(await BuildFormViewModelAsync(viewModel.Producto));

        _context.Add(viewModel.Producto);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Producto \"{viewModel.Producto.Nombre}\" creado correctamente.";
        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        var producto = await _context.Productos.FindAsync(id);
        if (producto is null) return NotFound();

        return View(await BuildFormViewModelAsync(producto));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductoFormViewModel viewModel, IFormFile? imagenFile)
    {
        if (id != viewModel.Producto.Id) return NotFound();

        // Procesar imagen nueva 
        if (imagenFile is not null && imagenFile.Length > 0)
        {
            viewModel.Producto.ImagenUrl = await GuardarImagenAsync(imagenFile);
            ModelState.Remove("Producto.ImagenUrl");
        }

        if (!ModelState.IsValid)
            return View(await BuildFormViewModelAsync(viewModel.Producto));

        var productoDB = await _context.Productos.FindAsync(id);
        if (productoDB is null) return NotFound();

        productoDB.Nombre       = viewModel.Producto.Nombre;
        productoDB.CategoriaId  = viewModel.Producto.CategoriaId;
        productoDB.Precio       = viewModel.Producto.Precio;
        productoDB.ImpuestoPorc = viewModel.Producto.ImpuestoPorc;
        productoDB.Stock        = viewModel.Producto.Stock;
        productoDB.Activo       = viewModel.Producto.Activo;

        // Solo actualizar imagen si se subió una nueva
        if (!string.IsNullOrWhiteSpace(viewModel.Producto.ImagenUrl))
            productoDB.ImagenUrl = viewModel.Producto.ImagenUrl;

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Producto \"{productoDB.Nombre}\" actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();

        var producto = await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (producto is null) return NotFound();

        return View(producto);
    }


    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto is not null)
        {
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Producto \"{producto.Nombre}\" eliminado correctamente.";
        }
        return RedirectToAction(nameof(Index));
    }

    // ── Helpers ──────

    private async Task<ProductoFormViewModel> BuildFormViewModelAsync(Producto producto)
    {
        return new ProductoFormViewModel
        {
            Producto   = producto,
            Categorias = await BuildCategoriasSelectAsync()
        };
    }

    private async Task<List<SelectListItem>> BuildCategoriasSelectAsync()
    {
        return await _context.Categorias
            .OrderBy(c => c.Nombre)
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Nombre })
            .ToListAsync();
    }

    /// Guarda el archivo en wwwroot/uploads/productos 
    private async Task<string> GuardarImagenAsync(IFormFile file)
    {
        var uploadPath = Path.Combine(_env.WebRootPath, "uploads", "productos");
        Directory.CreateDirectory(uploadPath);

        var extension = Path.GetExtension(file.FileName);
        var fileName  = $"{Guid.NewGuid()}{extension}";
        var fullPath  = Path.Combine(uploadPath, fileName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/uploads/productos/{fileName}";
    }
}
