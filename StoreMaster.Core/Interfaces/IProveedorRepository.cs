using StoreMaster.Core.Entities;

namespace StoreMaster.Core.Interfaces
{
    public interface IProveedorRepository: IGenericRepository<Proveedor>
    {
        Task<IEnumerable<Proveedor>> GetActivosAsync();
        Task<bool> ExisteNombreAsync(string nombre, int? excludeId = null);
    }
}
