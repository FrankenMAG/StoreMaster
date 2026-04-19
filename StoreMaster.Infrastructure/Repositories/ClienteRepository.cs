using Microsoft.EntityFrameworkCore;
using StoreMaster.Core.Entities;
using StoreMaster.Core.Interfaces;
using StoreMaster.Infrastructure.Data;

namespace StoreMaster.Infrastructure.Repositories;

public class ClienteRepository : GenericRepository<Cliente>, IClienteRepository
{
    public ClienteRepository(StoreDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Cliente>> GetActivosAsync()
        => await _dbSet
            .Where(c => c.Activo)
            .OrderBy(c => c.Nombre)
            .ToListAsync();

    public async Task<bool> ExisteEmailAsync(string email, int? excludeId = null)
        => await _dbSet.AnyAsync(c =>
            c.Email != null &&
            c.Email.ToLower() == email.ToLower() &&
            (!excludeId.HasValue || c.Id != excludeId.Value));
}