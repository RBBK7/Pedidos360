using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos360.Data;
using Pedidos360.Models;
using Pedidos360.ViewModels;
using X.PagedList.Extensions;

namespace Pedidos360.Controllers;

public class ClientesController : Controller
{
    private readonly Pedidos360Context _context;
    private const int PageSize = 10;

    public ClientesController(Pedidos360Context context)
    {
        _context = context;
    }


    public async Task<IActionResult> Index(string? busqueda, int page = 1)
    {
        var query = _context.Clientes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(busqueda))
            query = query.Where(c =>
                c.Nombre.Contains(busqueda) ||
                c.Cedula.Contains(busqueda));

        query = query.OrderBy(c => c.Nombre);

        var viewModel = new ClienteIndexViewModel
        {
            Clientes       = query.ToPagedList(page, PageSize),
            BusquedaFiltro = busqueda
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();

        var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
        if (cliente is null) return NotFound();

        return View(cliente);
    }


    public IActionResult Create()
    {
        return View(new Cliente());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Cliente cliente)
    {
        if (!ModelState.IsValid) return View(cliente);

        // Verificar cedula duplicada
        if (await _context.Clientes.AnyAsync(c => c.Cedula == cliente.Cedula))
        {
            ModelState.AddModelError("Cedula", "Ya existe un cliente con esa cédula/jurídica.");
            return View(cliente);
        }

        _context.Add(cliente);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Cliente \"{cliente.Nombre}\" creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente is null) return NotFound();

        return View(cliente);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Cliente cliente)
    {
        if (id != cliente.Id) return NotFound();
        if (!ModelState.IsValid) return View(cliente);

        // Verificar cedula duplicada en otro cliente
        if (await _context.Clientes.AnyAsync(c => c.Cedula == cliente.Cedula && c.Id != id))
        {
            ModelState.AddModelError("Cedula", "Ya existe un cliente con esa cédula/jurídica.");
            return View(cliente);
        }

        try
        {
            _context.Update(cliente);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Cliente \"{cliente.Nombre}\" actualizado correctamente.";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Clientes.AnyAsync(c => c.Id == id)) return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();

        var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
        if (cliente is null) return NotFound();

        return View(cliente);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente is not null)
        {
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Cliente \"{cliente.Nombre}\" eliminado correctamente.";
        }
        return RedirectToAction(nameof(Index));
    }
}
