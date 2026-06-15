using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Pedidos360.Models;



[ModelMetadataType(typeof(CategoriaMetadata))]
public partial class Categoria { }

[ModelMetadataType(typeof(ProductoMetadata))]
public partial class Producto { }

[ModelMetadataType(typeof(ClienteMetadata))]
public partial class Cliente { }

// ── Categoria 
public class CategoriaMetadata
{
    [Required(ErrorMessage = "El nombre es requerido.")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;
}

// ── Producto
public class ProductoMetadata
{
    [Required(ErrorMessage = "El nombre es requerido.")]
    [StringLength(150, ErrorMessage = "Máximo 150 caracteres.")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La categoría es requerida.")]
    [Display(Name = "Categoría")]
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "El precio es requerido.")]
    [Range(0.01, 99_999_999, ErrorMessage = "El precio debe ser mayor a 0.")]
    [Display(Name = "Precio")]
    [DataType(DataType.Currency)]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = "El impuesto es requerido.")]
    [Range(0, 100, ErrorMessage = "El impuesto debe estar entre 0 y 100.")]
    [Display(Name = "Impuesto (%)")]
    public decimal ImpuestoPorc { get; set; }

    [Required(ErrorMessage = "El stock es requerido.")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    [Display(Name = "Stock")]
    public int Stock { get; set; }

    [Required(ErrorMessage = "La imagen es requerida.")]
    [StringLength(500)]
    [Display(Name = "Imagen")]
    public string ImagenUrl { get; set; } = string.Empty;

    [Display(Name = "Activo")]
    public bool Activo { get; set; }
}

// ── Cliente
public class ClienteMetadata
{
    [Required(ErrorMessage = "El nombre es requerido.")]
    [StringLength(150, ErrorMessage = "Máximo 150 caracteres.")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La cédula/jurídica es requerida.")]
    [StringLength(20, ErrorMessage = "Máximo 20 caracteres.")]
    [Display(Name = "Cédula / Jurídica")]
    public string Cedula { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es requerido.")]
    [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
    [Display(Name = "Correo")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es requerido.")]
    [StringLength(20, ErrorMessage = "Máximo 20 caracteres.")]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dirección es requerida.")]
    [StringLength(250, ErrorMessage = "Máximo 250 caracteres.")]
    [Display(Name = "Dirección")]
    public string Direccion { get; set; } = string.Empty;
}
