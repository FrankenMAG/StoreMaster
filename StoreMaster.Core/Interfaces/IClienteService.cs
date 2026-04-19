using StoreMaster.Core.Entities;

namespace StoreMaster.Core.Interfaces;

public interface IClienteService
{
    Task<IEnumerable<Cliente>> GetAllAsync();
    Task<IEnumerable<Cliente>> GetActivosAsync();
    Task<Cliente?> GetByIdAsync(int id);
    Task<(bool Success, string Message)> CreateAsync(Cliente cliente);
    Task<(bool Success, string Message)> UpdateAsync(Cliente cliente);
    Task<(bool Success, string Message)> DeleteAsync(int id);
}