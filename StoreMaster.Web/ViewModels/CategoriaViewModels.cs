using System.ComponentModel.DataAnnotations;

namespace StoreMaster.Web.ViewModels
{
    public class CategoriaViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El nombre es requerido")]
        [MaxLength(100,ErrorMessage ="Máximo 100 caracteres")]
        [Display(Name ="Nombre")]
        public string Nombre { get; set; }=string.Empty;

        [MaxLength(255, ErrorMessage = "Maximo 255 caracteres")]
        [Display(Name = "Descripción")]
        public string? Descripcion {  get; set; }

        [Display(Name = "Activa")]
        public bool Activa { get; set; } = true;
        public DateTime CreadoEn {  get; set; }

        // Propiedad Calculada para mostrar cuantos productos tiene
        public int TotalProductos { get; set; }


    }
}
