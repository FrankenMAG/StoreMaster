using System;
using System.Collections.Generic;
using System.Text;

namespace StoreMaster.Core.Entities
{
    public class Factura : BaseEntity
    {
        public int Folio { get; set; }
        public string? Serie { get; set; }
        public DateTime FechaEmision { get; set; }
        public Guid? UUID { get; set; }
        public EstadoFactura Estado {  get; set; }= EstadoFactura.Vigente;
        public string RFCEmisor {  get; set; } = string.Empty;
        public string NombreEmisor { get; set; } = string.Empty;
        public string RegimenFiscalEmisor { get; set; } = string.Empty;

        public string RFCReceptor {  get; set; } = string.Empty;
        public string NombreReceptor {  get; set; } = string.Empty;
        public string RegimenFiscalReceptor {  get; set; } = string.Empty;
        public string UsoCFDI { get; set; } = string.Empty;
        public string CodigoPostalReceptor { get; set; } = string.Empty;

        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }
        public int VentaId { get; set; }
        public int ClienteId { get; set;}
        // Navegación
        public Cliente? Cliente { get; set; }
        public Venta? Venta { get; set; }
        public ICollection<FacturaConcepto> Conceptos { get; set; } = [];

    }
    public enum EstadoFactura
    {
        Vigente,
        Cancelada
    }
}
