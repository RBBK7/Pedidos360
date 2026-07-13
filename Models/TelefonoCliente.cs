namespace Pedidos360.Models;

public partial class TelefonoCliente
{
    public string Cedula { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public virtual Cliente? Cliente { get; set; }
}
