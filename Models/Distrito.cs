namespace Pedidos360.Models;

public partial class Distrito
{
    public int Id { get; set; }

    public int CantonId { get; set; }

    public string NombreDistrito { get; set; } = null!;

    public virtual Canton? Canton { get; set; }
}
