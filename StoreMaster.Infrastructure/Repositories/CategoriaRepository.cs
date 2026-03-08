using Microsoft.EntityFrameworkCore;
using StoreMaster.Core.Entities;
using StoreMaster.Core.Interfaces;
using StoreMaster.Infrastructure.Data;

namespace StoreMaster.Infrastructure.Repositories
{
    public class CategoriaRepository : GenericRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(StoreDbContext context) : base(context) { }
        public async Task<IEnumerable<Categoria>> GetActivasAsync()
            => await _dbSet
                .Where(c => c.Activa)
                .OrderBy(c => c.Nombre)
                .ToListAsync();

        public async Task<bool> ExisteNombreAsync(string nombre, int? excludeId = null)
            => await _dbSet.AnyAsync(c =>
                c.Nombre.ToLower() == nombre.ToLower() &&
                (!excludeId.HasValue || c.Id != excludeId.Value));       
    }
}
