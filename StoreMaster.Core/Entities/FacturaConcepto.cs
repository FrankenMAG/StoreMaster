using System;
using System.Collections.Generic;
using System.Text;

namespace StoreMaster.Core.Entities
{
    public class FacturaConcepto:BaseEntity
    {
        public string ClaveProdServ { get; set; } = string.Empty;
        public string ClaveUnidad { get; set; } = string.Empty;
        public string? NoIdentificacion { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Cantidad { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Importe { get; set; }
        public decimal? Descuento { get; set; }

        public decimal TasaIVA { get; set; }
        public decimal ImporteIVA { get; set; }

        public int FacturaId { get; set; }


    }

}
