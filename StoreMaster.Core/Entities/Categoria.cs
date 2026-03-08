using System;
using System.Collections.Generic;
using System.Text;

namespace StoreMaster.Core.Entities
{
    public class Categoria:BaseEntity
    {
        public string Nombre { get; set; }=string.Empty;
        public string? Descripcion {  get; set; }
        public bool Activa { get; set; } = true;

        // Navegacion
        public ICollection<Producto> Productos { get; set; } = [];
    }
}
