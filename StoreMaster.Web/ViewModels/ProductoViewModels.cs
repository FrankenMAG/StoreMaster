using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace StoreMaster.Web.ViewModels;

public class ProductoViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(500)]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [MaxLength(50)]
    [Display(Name = "Código de Barras")]
    public string? CodigoBarras { get; set; }

    [Required(ErrorMessage = "El precio es requerido")]
    [Range(0.01, 999999, ErrorMessage = "El precio debe ser mayor a cero")]
    [Display(Name = "Precio de Venta")]
    public decimal Precio { get; set; }

    [Range(0, 999999)]
    [Display(Name = "Precio de Compra")]
    public decimal? PrecioCompra { get; set; }

    [Required(ErrorMessage = "El stock es requerido")]
    [Range(0, 999999, ErrorMessage = "El stock no puede ser negativo")]
    [Display(Name = "Stock Actual")]
    public int Stock { get; set; }

    [Required(ErrorMessage = "El stock mínimo es requerido")]
    [Range(0, 999999)]
    [Display(Name = "Stock Mínimo")]
    public int StockMinimo { get; set; } = 5;

    [Display(Name = "Activo")]
    public bool Activo { get; set; } = true;

    // FK
    [Required(ErrorMessage = "La categoría es requerida")]
    [Display(Name = "Categoría")]
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "El proveedor es requerido")]
    [Display(Name = "Proveedor")]
    public int ProveedorId { get; set; }

    // Solo lectura — para mostrar en la tabla
    public string CategoriaNombre { get; set; } = string.Empty;
    public string ProveedorNombre { get; set; } = string.Empty;

    // Para los dropdowns
    public IEnumerable<SelectListItem> Categorias { get; set; } = [];
    public IEnumerable<SelectListItem> Proveedores { get; set; } = [];

    // Propiedad calculada
    public bool StockCritico => Stock <= StockMinimo;
    // Agregar al ProductoViewModel
    [Display(Name = "Imagen")]
    public IFormFile? ImagenFile { get; set; } // archivo que sube el usuario

    public string? ImagenUrl { get; set; } // ruta guardada en BD
}