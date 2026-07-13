namespace Pedidos360.Models;

public partial class Provincia
{
    public int Id { get; set; }

    public string NombreProvincia { get; set; } = null!;

    public virtual ICollection<Canton> Cantones { get; set; } = [];
}
