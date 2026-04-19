using Microsoft.EntityFrameworkCore;
using StoreMaster.Core.Entities;
using StoreMaster.Core.Interfaces;
using StoreMaster.Infrastructure.Data;

namespace StoreMaster.Infrastructure.Repositories;

public class VentaRepository : GenericRepository<Venta>, IVentaRepository
{
    public VentaRepository(StoreDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Venta>> GetAllWithRelationsAsync()
        => await _dbSet
            .Include(v => v.Cliente)
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .OrderByDescending(v => v.Fecha)
            .ToListAsync();

    public async Task<Venta?> GetByIdWithRelationsAsync(int id)
        => await _dbSet
            .Include(v => v.Cliente)
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(v => v.Id == id);

    public async Task<IEnumerable<Venta>> GetByFechaAsync(DateTime desde, DateTime hasta)
        => await _dbSet
            .Include(v => v.Cliente)
            .Include(v => v.Detalles)
            .Where(v => v.Fecha >= desde && v.Fecha <= hasta)
            .OrderByDescending(v => v.Fecha)
            .ToListAsync();

    public async Task<decimal> GetTotalVentasHoyAsync()
    {
        var hoy = DateTime.UtcNow.Date;
        return await _dbSet
            .Where(v => v.Fecha.Date == hoy && v.Estado == EstadoVenta.Completada)
            .SumAsync(v => v.Total);
    }

    public async Task<int> GetTotalVentasHoyCountAsync()
    {
        var hoy = DateTime.UtcNow.Date;
        return await _dbSet
            .Where(v => v.Fecha.Date == hoy && v.Estado == EstadoVenta.Completada)
            .CountAsync();
    }
}