using StoreMaster.Core.Entities;

namespace StoreMaster.Core.Interfaces;

public interface IClienteRepository : IGenericRepository<Cliente>
{
    Task<IEnumerable<Cliente>> GetActivosAsync();
    Task<bool> ExisteEmailAsync(string email, int? excludeId = null);
}