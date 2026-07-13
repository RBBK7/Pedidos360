namespace Pedidos360.Models;

public partial class Estado
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Cliente> Clientes { get; set; } = [];

    public virtual ICollection<Pedido> Pedidos { get; set; } = [];

    public virtual ICollection<Producto> Productos { get; set; } = [];
}