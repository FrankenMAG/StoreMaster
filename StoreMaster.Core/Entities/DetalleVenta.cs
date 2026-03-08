using System;
using System.Collections.Generic;
using System.Text;

namespace StoreMaster.Core.Entities
{
    public class DetalleVenta : BaseEntity
    {
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;

        // FK
        public int VentaId { get; set; }
        public int ProductoId { get; set; }

        // Navegación
        public Venta Venta { get; set; } = null!;
        public Producto Producto { get; set; } = null!;
    }
}
