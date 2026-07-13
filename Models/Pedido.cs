namespace Pedidos360.Models;

public partial class Pedido
{
    public int Id { get; set; }

    public string ClienteId { get; set; } = null!;

    public int EstadoId { get; set; }

    public int RolId { get; set; }

    public DateTime Fecha { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Impuesto { get; set; }

    public decimal Descuento { get; set; }

    public decimal Total { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual Estado? Estado { get; set; }

    public virtual ICollection<DetallePedido> Detalles { get; set; } = [];
}