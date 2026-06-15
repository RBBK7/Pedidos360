using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos360.Data;
using Pedidos360.Models;

namespace Pedidos360.Controllers;

public class CategoriasController : Controller
{
    private readonly Pedidos360Context _context;

    public CategoriasController(Pedidos360Context context)
    {
        _context = context;
    }

    // GET: /Categorias
    public async Task<IActionResult> Index()
    {
        return View(await _context.Categorias.OrderBy(c => c.Nombre).ToListAsync());
    }

    // GET: /Categorias/Details/
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();

        var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
        if (categoria is null) return NotFound();

        return View(categoria);
    }

    // GET: /Categorias/Create
    public IActionResult Create()
    {
        return View(new Categoria());
    }

    // POST: /Categorias/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Categoria categoria)
    {
        if (!ModelState.IsValid) return View(categoria);

        _context.Add(categoria);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Categoría \"{categoria.Nombre}\" creada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria is null) return NotFound();

        return View(categoria);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Categoria categoria)
    {
        if (id != categoria.Id) return NotFound();
        if (!ModelState.IsValid) return View(categoria);

        try
        {
            _context.Update(categoria);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Categoría \"{categoria.Nombre}\" actualizada correctamente.";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Categorias.AnyAsync(c => c.Id == id)) return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();

        var categoria = await _context.Categorias
            .Include(c => c.Productos)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (categoria is null) return NotFound();

        return View(categoria);
    }


    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var categoria = await _context.Categorias
            .Include(c => c.Productos)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria is not null)
        {
            if (categoria.Productos.Any())
            {
                TempData["ErrorMessage"] = "No se puede eliminar la categoría porque tiene productos asociados.";
                return RedirectToAction(nameof(Index));
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Categoría \"{categoria.Nombre}\" eliminada correctamente.";
        }

        return RedirectToAction(nameof(Index));
    }
}
