using System.ComponentModel.DataAnnotations;

namespace Pedidos360.Models;

public partial class Cliente
{
    [Required(ErrorMessage = "La cédula es obligatoria")]
    [StringLength(20, MinimumLength = 9, ErrorMessage = "La cedula debe tener entre 9 y 20 caracteres")]
    [RegularExpression(@"^\d{9,12}$", ErrorMessage = "La cedula solo debe contener numeros entre 9 y 20 caracteres")]
    public string Cedula { get; set; } = null!;

    public int EstadoId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(80, ErrorMessage = "Maximo 80 caracteres")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El Primer Apellido es obligatorio")]
    [StringLength(80, ErrorMessage = "Maximo 80 caracteres")]
    public string ApellidoPaterno { get; set; } = null!;

    [StringLength(80, ErrorMessage = "Maximo 80 caracteres")]
    public string? ApellidoMaterno { get; set; }

    public virtual Estado? Estado { get; set; }

    public virtual ICollection<CorreoCliente> Correos { get; set; } = [];

   
    public virtual ICollection<TelefonoCliente> Telefonos { get; set; } = [];

    public virtual ICollection<Direccion> Direcciones { get; set; } = [];

    public virtual ICollection<Pedido> Pedidos { get; set; } = [];
}