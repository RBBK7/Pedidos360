namespace Pedidos360.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int CategoriaId { get; set; }

    public decimal Precio { get; set; }

    public decimal ImpuestoPorc { get; set; }

    public int Stock { get; set; }

    public string ImagenUrl { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual Categoria? Categoria { get; set; }
}
