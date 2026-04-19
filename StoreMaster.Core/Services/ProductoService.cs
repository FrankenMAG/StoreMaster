using StoreMaster.Core.Entities;
using StoreMaster.Core.Interfaces;

namespace StoreMaster.Core.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _repository;

    public ProductoService(IProductoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Producto>> GetAllAsync()
        => await _repository.GetAllWithRelationsAsync();

    public async Task<IEnumerable<Producto>> GetActivosAsync()
        => await _repository.GetActivosAsync();

    public async Task<IEnumerable<Producto>> GetStockBajoAsync()
        => await _repository.GetStockBajoAsync();

    public async Task<Producto?> GetByIdAsync(int id)
        => await _repository.GetByIdWithRelationsAsync(id);

    public async Task<(bool Success, string Message)> CreateAsync(Producto producto)
    {
        // Regla: código de barras único si se especificó
        if (!string.IsNullOrEmpty(producto.CodigoBarras))
        {
            if (await _repository.ExisteCodigoBarrasAsync(producto.CodigoBarras))
                return (false, $"Ya existe un producto con el código '{producto.CodigoBarras}'.");
        }

        // Regla: el stock inicial no puede ser negativo
        if (producto.Stock < 0)
            return (false, "El stock inicial no puede ser negativo.");

        // Regla: el precio debe ser mayor a cero
        if (producto.Precio <= 0)
            return (false, "El precio debe ser mayor a cero.");

        await _repository.AddAsync(producto);
        return (true, $"Producto '{producto.Nombre}' creado correctamente.");
    }

    public async Task<(bool Success, string Message)> UpdateAsync(Producto producto)
    {
        if (!string.IsNullOrEmpty(producto.CodigoBarras))
        {
            if (await _repository.ExisteCodigoBarrasAsync(producto.CodigoBarras, producto.Id))
                return (false, $"Ya existe un producto con el código '{producto.CodigoBarras}'.");
        }

        if (producto.Precio <= 0)
            return (false, "El precio debe ser mayor a cero.");

        producto.ModificadoEn = DateTime.UtcNow;
        await _repository.UpdateAsync(producto);
        return (true, $"Producto '{producto.Nombre}' actualizado correctamente.");
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var producto = await _repository.GetByIdAsync(id);

        if (producto == null)
            return (false, "Producto no encontrado.");

        producto.Eliminado = true;
        await _repository.UpdateAsync(producto);
        return (true, $"Producto '{producto.Nombre}' eliminado correctamente.");
    }

    public async Task<(bool Success, string Message)> ActualizarStockAsync(
        int id, int cantidad, bool esEntrada)
    {
        var producto = await _repository.GetByIdAsync(id);

        if (producto == null)
            return (false, "Producto no encontrado.");

        if (esEntrada)
        {
            producto.Stock += cantidad;
        }
        else
        {
            // Regla: no puedes sacar más de lo que hay
            if (producto.Stock < cantidad)
                return (false, $"Stock insuficiente. Stock actual: {producto.Stock}");

            producto.Stock -= cantidad;
        }

        producto.ModificadoEn = DateTime.UtcNow;
        await _repository.UpdateAsync(producto);

        var tipo = esEntrada ? "entrada" : "salida";
        return (true, $"Stock actualizado. {tipo} de {cantidad} unidades. Stock actual: {producto.Stock}");
    }
    public async Task<int> GetTotalActivosAsync()
    => await _repository.GetTotalActivosAsync();

    public async Task<IEnumerable<(string Nombre, int Total)>> GetTopProductosAsync(int top = 5)
        => await _repository.GetTopProductosAsync(top);
}