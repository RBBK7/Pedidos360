namespace Pedidos360.Models;

public partial class Canton
{
    public int Id { get; set; }

    public int ProvinciaId { get; set; }

    public string NombreCanton { get; set; } = null!;

    public virtual Provincia? Provincia { get; set; }

    public virtual ICollection<Distrito> Distritos { get; set; } = [];
}
