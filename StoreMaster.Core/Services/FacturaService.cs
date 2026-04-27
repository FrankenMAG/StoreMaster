using StoreMaster.Core.Entities;
using StoreMaster.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreMaster.Core.Services
{
    public class FacturaService : IFacturaService
    {
        private readonly IFacturaRepository _facturaRepository;
        private readonly IVentaRepository _ventaRepository;

        public FacturaService(
            IFacturaRepository facturaRepository,
            IVentaRepository ventaRepository)
        {
            _ventaRepository = ventaRepository;
            _facturaRepository = facturaRepository;
        }
        public async Task<IEnumerable<Factura>> GetAllAsync()
            => await _facturaRepository.GetAllAsync();

        public async Task<Factura?> GetByVentaIdAsync(int ventaid)
            => await _facturaRepository.GetByVentaIdAsync(ventaid);

        public async Task<Factura?> GetByIdAsync(int id)
            => await _facturaRepository.GetByIdWithRelationsAsync(id);


        public async Task<(bool Success, string Message, int FacturaId)> CrearFacturaAsync(int ventaId, string serie)
        {
            Factura factura;
            var venta = await _ventaRepository.GetByIdWithRelationsAsync(ventaId);

            if (venta == null) 
                return (false, "Venta no encontrada", 0);

            if (venta.Estado != EstadoVenta.Completada)
                return (false, "Solo se pueden facturar ventas completadas", 0);

            var facturaExistente = await _facturaRepository.GetByVentaIdAsync(ventaId);
            if (facturaExistente != null)
                return (false, "Esta venta ya tiene una factura", 0);

            var folio = await _facturaRepository.GetNextFolioAsync(serie);

            factura = new Factura
            {
                Folio = folio,
                Serie = serie,
                FechaEmision = DateTime.UtcNow,
                UUID = null,
                Estado = EstadoFactura.Vigente,

                //Datos fijos por lo pronto, hasta tener bien implementado
                RFCEmisor = "XAXX010101000",
                NombreEmisor = "STOREMASTER",
                RegimenFiscalEmisor = "601",

                //Datos del receptor(cliente)
                RFCReceptor = venta.Cliente?.RFC ?? "XAX010101000",
                NombreReceptor = venta.Cliente?.NombreCompleto ?? "Público General",
                RegimenFiscalReceptor = venta.Cliente?.RegimenFiscal ?? "616",
                UsoCFDI = venta.Cliente?.UsoCFDI ?? "S01",
                CodigoPostalReceptor = venta.Cliente?.CodigoPostalFiscal ?? "00000",

                // Importes de la venta
                Subtotal = venta.Subtotal,
                IVA = venta.Impuesto,
                Total = venta.Total,

                //Relaciones
                VentaId = ventaId,
                ClienteId = venta.ClienteId ?? 0
            };
            factura.Conceptos= venta.Detalles.Select(d=> new FacturaConcepto
            {
                ClaveProdServ="01010101",
                ClaveUnidad="H87",
                NoIdentificacion=d.ProductoId.ToString(),
                Descripcion=d.Producto?.Nombre??"Producto",
                Cantidad=d.Cantidad,
                ValorUnitario=d.PrecioUnitario,
                Importe=d.Cantidad*d.PrecioUnitario,
                TasaIVA=0.16m,
                ImporteIVA=(d.Cantidad*d.PrecioUnitario)*0.16m
            }).ToList();
            await _facturaRepository.AddAsync(factura);
            return (true, $"Factura {serie}-{folio} generada correctamente.", factura.Id);
        }
        public async Task<(bool Success, string Message)> CancelarFacturaAsync(int id)
        {
            var factura = await _facturaRepository.GetByIdAsync(id);

            if (factura == null)
                return (false, "Factura no encontrada.");

            if (factura.Estado == EstadoFactura.Cancelada)
                return (false, "La factura ya está cancelada.");



            factura.Estado = EstadoFactura.Cancelada;
            await _facturaRepository.UpdateAsync(factura);

            return (true, $"Factura #{factura.Id} cancelada.");
        }

    }
}
