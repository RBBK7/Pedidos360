namespace Pedidos360.Models;

public partial class Cliente
{
    public string Cedula { get; set; } = null!;

    public int EstadoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public virtual Estado? Estado { get; set; }

    public virtual ICollection<CorreoCliente> Correos { get; set; } = [];

    public virtual ICollection<TelefonoCliente> Telefonos { get; set; } = [];

    public virtual ICollection<Direccion> Direcciones { get; set; } = [];

    public virtual ICollection<Pedido> Pedidos { get; set; } = [];
}
