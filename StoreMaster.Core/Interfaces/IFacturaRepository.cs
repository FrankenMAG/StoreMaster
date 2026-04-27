using StoreMaster.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreMaster.Core.Interfaces
{
    public interface IFacturaRepository : IGenericRepository<Factura>
    {
        Task<IEnumerable<Factura>> GetAllWithRelationsAsync();
        Task<Factura?> GetByIdWithRelationsAsync(int id);
        Task<Factura?> GetByVentaIdAsync(int ventaId);
        Task<int> GetNextFolioAsync(string serie);
    }
}
