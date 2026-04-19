using StoreMaster.Core.Entities;

namespace StoreMaster.Core.Interfaces;

public interface IProductoRepository : IGenericRepository<Producto>
{
    Task<IEnumerable<Producto>> GetAllWithRelationsAsync();
    Task<Producto?> GetByIdWithRelationsAsync(int id);
    Task<IEnumerable<Producto>> GetActivosAsync();
    Task<IEnumerable<Producto>> GetStockBajoAsync();
    Task<bool> ExisteCodigoBarrasAsync(string codigo, int? excludeId = null);
}