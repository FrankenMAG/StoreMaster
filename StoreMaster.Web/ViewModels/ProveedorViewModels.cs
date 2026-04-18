using System.ComponentModel.DataAnnotations;

namespace StoreMaster.Web.ViewModels;

public class ProveedorViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(100)]
    [Display(Name = "Contacto")]
    public string? Contacto { get; set; }

    [MaxLength(20)]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    [EmailAddress(ErrorMessage = "Email inválido")]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [MaxLength(255)]
    [Display(Name = "Dirección")]
    public string? Direccion { get; set; }

    [Display(Name = "Activo")]
    public bool Activo { get; set; } = true;

    public DateTime CreadoEn { get; set; }
    public int TotalProductos { get; set; }
}