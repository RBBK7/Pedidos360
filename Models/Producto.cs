using System.ComponentModel.DataAnnotations;

namespace Pedidos360.Models;

public partial class Producto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(150, ErrorMessage = "Máximo 150 caracteres")]
    public string Nombre { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría")]
    public int CategoriaId { get; set; }

    [Range(0.01, 999999, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal Precio { get; set; }

    [Range(0, 13, ErrorMessage = "El impuesto debe estar entre 0% y 13%")]
    public decimal ImpuestoPorc { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser mayor a 0")]
    public int Stock { get; set; }

    public string Imagen { get; set; } = null!;

    public int EstadoId { get; set; }

    public virtual Categoria? Categoria { get; set; }

    public virtual Estado? Estado { get; set; }

    public virtual ICollection<DetallePedido> DetallesPedido { get; set; } = [];
}