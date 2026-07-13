namespace Pedidos360.Models;

public partial class Direccion
{
    public int Id { get; set; }

    public string Cedula { get; set; } = null!;

    public int ProvinciaId { get; set; }

    public int CantonId { get; set; }

    public int DistritoId { get; set; }

    public string? OtrasSenas { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual Provincia? Provincia { get; set; }

    public virtual Canton? Canton { get; set; }

    public virtual Distrito? Distrito { get; set; }
}
