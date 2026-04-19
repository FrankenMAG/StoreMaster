using StoreMaster.Core.Entities;
using StoreMaster.Core.Interfaces;

namespace StoreMaster.Core.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _repository;

    public ClienteService(IClienteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Cliente>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<IEnumerable<Cliente>> GetActivosAsync()
        => await _repository.GetActivosAsync();

    public async Task<Cliente?> GetByIdAsync(int id)
        => await _repository.GetByIdAsync(id);

    public async Task<(bool Success, string Message)> CreateAsync(Cliente cliente)
    {
        if (!string.IsNullOrEmpty(cliente.Email))
        {
            if (await _repository.ExisteEmailAsync(cliente.Email))
                return (false, $"Ya existe un cliente con el email '{cliente.Email}'.");
        }

        await _repository.AddAsync(cliente);
        return (true, $"Cliente '{cliente.Nombre}' creado correctamente.");
    }

    public async Task<(bool Success, string Message)> UpdateAsync(Cliente cliente)
    {
        if (!string.IsNullOrEmpty(cliente.Email))
        {
            if (await _repository.ExisteEmailAsync(cliente.Email, cliente.Id))
                return (false, $"Ya existe un cliente con el email '{cliente.Email}'.");
        }

        cliente.ModificadoEn = DateTime.UtcNow;
        await _repository.UpdateAsync(cliente);
        return (true, $"Cliente '{cliente.Nombre}' actualizado correctamente.");
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var cliente = await _repository.GetByIdAsync(id);

        if (cliente == null)
            return (false, "Cliente no encontrado.");

        cliente.Eliminado = true;
        await _repository.UpdateAsync(cliente);
        return (true, $"Cliente '{cliente.Nombre}' eliminado correctamente.");
    }

    public async Task<int> GetTotalActivosAsync()
        => await _repository.GetTotalActivosAsync();
}