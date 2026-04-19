using StoreMaster.Core.Entities;

namespace StoreMaster.Core.Interfaces;

public interface IVentaRepository : IGenericRepository<Venta>
{
    Task<IEnumerable<Venta>> GetAllWithRelationsAsync();
    Task<Venta?> GetByIdWithRelationsAsync(int id);
    Task<IEnumerable<Venta>> GetByFechaAsync(DateTime desde, DateTime hasta);
    Task<decimal> GetTotalVentasHoyAsync();
    Task<int> GetTotalVentasHoyCountAsync();
}