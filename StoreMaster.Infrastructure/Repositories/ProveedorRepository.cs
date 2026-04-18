using Microsoft.EntityFrameworkCore;
using StoreMaster.Core.Entities;
using StoreMaster.Core.Interfaces;
using StoreMaster.Infrastructure.Data;

namespace StoreMaster.Infrastructure.Repositories
{
    public class ProveedorRepository : GenericRepository<Proveedor>, IProveedorRepository
    {
        public ProveedorRepository(StoreDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Proveedor>> GetActivosAsync()
            => await _dbSet
            .Where(p => p.Activo)
            .OrderBy(p => p.Nombre)
            .ToListAsync();
        public async Task<bool> ExisteNombreAsync(string nombre, int? excludeId = null)
        => await _dbSet.AnyAsync(p =>
            p.Nombre.ToLower() == nombre.ToLower() &&
            (!excludeId.HasValue || p.Id != excludeId.Value));
    }
}
