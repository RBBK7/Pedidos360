namespace Pedidos360.Models;

public partial class CorreoCliente
{
    public string Cedula { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public virtual Cliente? Cliente { get; set; }
}
