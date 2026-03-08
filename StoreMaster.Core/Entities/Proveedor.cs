using System;
using System.Collections.Generic;
using System.Text;

namespace StoreMaster.Core.Entities
{
    public class Proveedor:BaseEntity
    {
        public string Nombre { get; set; }=string.Empty;    
        public string? Contacto { get; set; }
        public string? Telefono { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Direccion { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;

        // Navegacion
        public ICollection<Producto> Productos { get; set; } = [];
    }
}
