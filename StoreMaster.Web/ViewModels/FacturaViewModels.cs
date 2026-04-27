using StoreMaster.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Pkcs;

namespace StoreMaster.Web.ViewModels
{
    public class FacturaViewModel
    {
        public int Id { get; set; }
        public string FolioCompleto => $"{Serie}-{Folio}";
        public string Serie { get; set; } = string.Empty;
        public int Folio { get; set; }
        public DateTime FechaEmision { get; set; }
        public string NombreReceptor { get; set; } = string.Empty;
        public string RFCReceptor { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }
        public EstadoFactura Estado { get; set; }
        public int VentaId {  get; set; }
        public List<FacturaConceptoViewModel> Conceptos { get; set; } = [];

    }
    public class FacturaConceptoViewModel
    {
        public string Descripcion { get; set; }= string.Empty;
        public decimal Cantidad { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Importe { get; set; }
        public decimal ImporteIVA { get; set; }
    }
    public class CrearFacturaViewModel
    {
        [Required]
        public int VentaId { get; set; }

        [Required]
        public string Serie { get; set; } = "A";
    }
}
