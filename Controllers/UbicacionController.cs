using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos360.Data;

namespace Pedidos360.Controllers;

[Route("api/ubicacion")]
public class UbicacionController : Controller
{
    private readonly Pedidos360Context _context;

    public UbicacionController(Pedidos360Context context)
    {
        _context = context;
    }

    // GET: /api/ubicacion/cantones
    [HttpGet("cantones")]
    public async Task<IActionResult> Cantones(int provinciaId)
    {
        var cantones = await _context.Cantones
            .Where(c => c.ProvinciaId == provinciaId)
            .OrderBy(c => c.NombreCanton)
            .Select(c => new { id = c.Id, nombre = c.NombreCanton })
            .ToListAsync();

        return Json(cantones);
    }

    // GET: /api/ubicacion/distritos
    [HttpGet("distritos")]
    public async Task<IActionResult> Distritos(int cantonId)
    {
        var distritos = await _context.Distritos
            .Where(d => d.CantonId == cantonId)
            .OrderBy(d => d.NombreDistrito)
            .Select(d => new { id = d.Id, nombre = d.NombreDistrito })
            .ToListAsync();

        return Json(distritos);
    }
}