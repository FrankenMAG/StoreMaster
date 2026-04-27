using StoreMaster.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreMaster.Core.Interfaces
{
    public interface IFacturaService
    {
        Task<IEnumerable<Factura>> GetAllAsync();
        Task<Factura?> GetByIdAsync(int id);
        Task<Factura?> GetByVentaIdAsync(int ventaid);

        Task<(bool Success, string Message, int FacturaId)> CrearFacturaAsync(int ventaId, string serie);
        Task<(bool Success, string Message)> CancelarFacturaAsync(int id);
    }
}
