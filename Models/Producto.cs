namespace Pedidos360.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int CategoriaId { get; set; }

    public decimal Precio { get; set; }

    public decimal ImpuestoPorc { get; set; }

    public int Stock { get; set; }

    public string Imagen { get; set; } = null!;

    public int EstadoId { get; set; }

    public virtual Categoria? Categoria { get; set; }

    public virtual Estado? Estado { get; set; }

    public virtual ICollection<DetallePedido> DetallesPedido { get; set; } = [];
}