using StoreMaster.Core.Entities;
using StoreMaster.Core.Interfaces;

namespace StoreMaster.Core.Services;

public class ProveedorService : IProveedorService
{
    private readonly IProveedorRepository _repository;

    public ProveedorService(IProveedorRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Proveedor>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<IEnumerable<Proveedor>> GetActivosAsync()
        => await _repository.GetActivosAsync();

    public async Task<Proveedor?> GetByIdAsync(int id)
        => await _repository.GetByIdAsync(id);

    public async Task<(bool Success, string Message)> CreateAsync(Proveedor proveedor)
    {
        if (await _repository.ExisteNombreAsync(proveedor.Nombre))
            return (false, $"Ya existe un proveedor con el nombre '{proveedor.Nombre}'.");

        await _repository.AddAsync(proveedor);
        return (true, $"Proveedor '{proveedor.Nombre}' creado correctamente.");
    }

    public async Task<(bool Success, string Message)> UpdateAsync(Proveedor proveedor)
    {
        if (await _repository.ExisteNombreAsync(proveedor.Nombre, proveedor.Id))
            return (false, $"Ya existe un proveedor con el nombre '{proveedor.Nombre}'.");

        proveedor.ModificadoEn = DateTime.UtcNow;
        await _repository.UpdateAsync(proveedor);
        return (true, $"Proveedor '{proveedor.Nombre}' actualizado correctamente.");
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var proveedor = await _repository.GetByIdAsync(id);

        if (proveedor == null)
            return (false, "Proveedor no encontrado.");

        proveedor.Eliminado = true;
        await _repository.UpdateAsync(proveedor);
        return (true, $"Proveedor '{proveedor.Nombre}' eliminado correctamente.");
    }
}