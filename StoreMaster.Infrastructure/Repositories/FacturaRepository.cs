using Microsoft.EntityFrameworkCore;
using StoreMaster.Core.Entities;
using StoreMaster.Core.Interfaces;
using StoreMaster.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreMaster.Infrastructure.Repositories
{
    public class FacturaRepository : GenericRepository<Factura>, IFacturaRepository
    {
        public FacturaRepository(StoreDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Factura>> GetAllWithRelationsAsync()
        => await _dbSet
            .Include(f => f.Cliente)
            .Include(f => f.Venta)
            .Include(f => f.Conceptos)
            .OrderByDescending(f => f.FechaEmision)
            .ToListAsync();
        public async Task<Factura?> GetByIdWithRelationsAsync(int id)
        => await _dbSet
            .Include(f => f.Cliente)
            .Include(f => f.Venta)
            .Include(f => f.Conceptos)
            .FirstOrDefaultAsync(f => f.Id == id);

        public async Task<Factura?> GetByVentaIdAsync(int ventaId)
        => await _dbSet
                    .Include(f => f.Cliente)
                    .Include(f => f.Venta)
                    .Include(f => f.Conceptos)
                    
                    .FirstOrDefaultAsync(f => f.VentaId== ventaId
                                            && f.Estado!=EstadoFactura.Cancelada);

        public async Task<int> GetNextFolioAsync(string serie)
        {
            var hayFacturas=await _dbSet
            .AnyAsync(f => f.Serie == serie);

            if (!hayFacturas)
                return 1;

            var maxFolio = await _dbSet
                .Where(f=>f.Serie==serie)
                .MaxAsync(f => f.Folio);
            return maxFolio+1;
        }
    }
}
