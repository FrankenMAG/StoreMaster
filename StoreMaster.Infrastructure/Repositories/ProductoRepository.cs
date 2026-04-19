using Microsoft.EntityFrameworkCore;
using StoreMaster.Core.Entities;
using StoreMaster.Core.Interfaces;
using StoreMaster.Infrastructure.Data;

namespace StoreMaster.Infrastructure.Repositories;

public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
{
    public ProductoRepository(StoreDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Producto>> GetAllWithRelationsAsync()
        => await _dbSet
            .Include(p => p.Categoria)
            .Include(p => p.Proveedor)
            .OrderBy(p => p.Nombre)
            .ToListAsync();

    public async Task<Producto?> GetByIdWithRelationsAsync(int id)
        => await _dbSet
            .Include(p => p.Categoria)
            .Include(p => p.Proveedor)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Producto>> GetActivosAsync()
        => await _dbSet
            .Include(p => p.Categoria)
            .Include(p => p.Proveedor)
            .Where(p => p.Activo)
            .OrderBy(p => p.Nombre)
            .ToListAsync();

    public async Task<IEnumerable<Producto>> GetStockBajoAsync()
        => await _dbSet
            .Include(p => p.Categoria)
            .Where(p => p.Stock <= p.StockMinimo)
            .OrderBy(p => p.Stock)
            .ToListAsync();

    public async Task<bool> ExisteCodigoBarrasAsync(string codigo, int? excludeId = null)
        => await _dbSet.AnyAsync(p =>
            p.CodigoBarras == codigo &&
            (!excludeId.HasValue || p.Id != excludeId.Value));

    public async Task<int> GetTotalActivosAsync()
        => await _dbSet.CountAsync(p => p.Activo);

    public async Task<IEnumerable<(string Nombre, int Total)>> GetTopProductosAsync(int top = 5)
    {
        var resultados = await _context.DetallesVenta
            .Include(d => d.Producto)
            .GroupBy(d => new { d.ProductoId, d.Producto.Nombre })
            .Select(g => new
            {
                Nombre = g.Key.Nombre,
                Total = g.Sum(d => d.Cantidad)
            })
            .OrderByDescending(x => x.Total)
            .Take(top)
            .ToListAsync();

        return resultados.Select(x => (x.Nombre, x.Total));
    }


}