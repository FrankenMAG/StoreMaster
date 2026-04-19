using System.ComponentModel.DataAnnotations;

namespace StoreMaster.Web.ViewModels;

public class ClienteViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [MaxLength(100)]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(100)]
    [Display(Name = "Apellido")]
    public string? Apellido { get; set; }

    [MaxLength(20)]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    [EmailAddress(ErrorMessage = "Email inválido")]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [MaxLength(255)]
    [Display(Name = "Dirección")]
    public string? Direccion { get; set; }

    [MaxLength(255)]
    [Display(Name = "RFC")]
    public string? RFC { get; set; }


    [MaxLength(255)]
    [Display(Name = "Razón Social")]
    public string? RazonSocial { get; set; }

    [MaxLength(255)]
    [Display(Name = "Regimen Fiscal")]
    public string? RegimenFIscal { get; set; }

    [MaxLength(255)]
    [Display(Name = "Codigo Postal Fiscal")]
    public string? CodigoPostalFiscal { get; set; }


    [Display(Name = "Activo")]
    public bool Activo { get; set; } = true;

    public DateTime CreadoEn { get; set; }
    public string NombreCompleto => $"{Nombre} {Apellido}".Trim();
    public int TotalCompras { get; set; }
}