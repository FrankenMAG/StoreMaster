using StoreMaster.Core.Entities;

namespace StoreMaster.Core.Interfaces;

public interface IProductoService
{
    Task<IEnumerable<Producto>> GetAllAsync();
    Task<IEnumerable<Producto>> GetActivosAsync();
    Task<IEnumerable<Producto>> GetStockBajoAsync();
    Task<Producto?> GetByIdAsync(int id);
    Task<(bool Success, string Message)> CreateAsync(Producto producto);
    Task<(bool Success, string Message)> UpdateAsync(Producto producto);
    Task<(bool Success, string Message)> DeleteAsync(int id);
    Task<(bool Success, string Message)> ActualizarStockAsync(int id, int cantidad, bool esEntrada);
    Task<int> GetTotalActivosAsync();
    Task<IEnumerable<(string Nombre, int Total)>> GetTopProductosAsync(int top = 5);
}