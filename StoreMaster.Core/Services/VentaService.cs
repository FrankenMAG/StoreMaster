using StoreMaster.Core.Entities;
using StoreMaster.Core.Interfaces;

namespace StoreMaster.Core.Services;

public class VentaService : IVentaService
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IProductoRepository _productoRepository;

    public VentaService(
        IVentaRepository ventaRepository,
        IProductoRepository productoRepository)
    {
        _ventaRepository = ventaRepository;
        _productoRepository = productoRepository;
    }

    public async Task<IEnumerable<Venta>> GetAllAsync()
        => await _ventaRepository.GetAllWithRelationsAsync();

    public async Task<Venta?> GetByIdAsync(int id)
        => await _ventaRepository.GetByIdWithRelationsAsync(id);

    public async Task<IEnumerable<Venta>> GetByFechaAsync(DateTime desde, DateTime hasta)
        => await _ventaRepository.GetByFechaAsync(desde, hasta);

    public async Task<decimal> GetTotalVentasHoyAsync()
        => await _ventaRepository.GetTotalVentasHoyAsync();

    public async Task<int> GetTotalVentasHoyCountAsync()
        => await _ventaRepository.GetTotalVentasHoyCountAsync();

    public async Task<(bool Success, string Message, int VentaId)> CrearVentaAsync(
        Carrito carrito, int? clienteId)
    {
        // Regla: el carrito no puede estar vacío
        if (!carrito.Items.Any())
            return (false, "El carrito está vacío.", 0);

        // Verificar stock de todos los productos antes de crear la venta
        foreach (var item in carrito.Items)
        {
            var producto = await _productoRepository.GetByIdAsync(item.ProductoId);

            if (producto == null)
                return (false, $"El producto '{item.Nombre}' ya no existe.", 0);

            if (producto.Stock < item.Cantidad)
                return (false,
                    $"Stock insuficiente para '{item.Nombre}'. " +
                    $"Stock disponible: {producto.Stock}.", 0);
        }

        // Crear la venta
        var venta = new Venta
        {
            Fecha = DateTime.UtcNow,
            ClienteId = clienteId,
            Subtotal = carrito.Subtotal,
            Impuesto = carrito.Impuesto,
            Total = carrito.Total,
            Estado = EstadoVenta.Completada,
            Detalles = carrito.Items.Select(item => new DetalleVenta
            {
                ProductoId = item.ProductoId,
                Cantidad = item.Cantidad,
                PrecioUnitario = item.PrecioUnitario
            }).ToList()
        };

        await _ventaRepository.AddAsync(venta);

        // Descontar stock de cada producto
        foreach (var item in carrito.Items)
        {
            var producto = await _productoRepository.GetByIdAsync(item.ProductoId);
            if (producto != null)
            {
                producto.Stock -= item.Cantidad;
                await _productoRepository.UpdateAsync(producto);
            }
        }

        return (true, "Venta registrada correctamente.", venta.Id);
    }

    public async Task<(bool Success, string Message)> CancelarVentaAsync(int id)
    {
        var venta = await _ventaRepository.GetByIdWithRelationsAsync(id);

        if (venta == null)
            return (false, "Venta no encontrada.");

        if (venta.Estado == EstadoVenta.Cancelada)
            return (false, "La venta ya está cancelada.");

        // Devolver stock de cada producto
        foreach (var detalle in venta.Detalles)
        {
            var producto = await _productoRepository.GetByIdAsync(detalle.ProductoId);
            if (producto != null)
            {
                producto.Stock += detalle.Cantidad;
                await _productoRepository.UpdateAsync(producto);
            }
        }

        venta.Estado = EstadoVenta.Cancelada;
        await _ventaRepository.UpdateAsync(venta);

        return (true, $"Venta #{venta.Id} cancelada. Stock restaurado.");
    }
    public async Task<IEnumerable<Venta>> GetVentasUltimosDiasAsync(int dias = 7)
    => await _ventaRepository.GetVentasUltimosDiasAsync(dias);

    public async Task<IEnumerable<Venta>> GetVentasHoyAsync()
        => await _ventaRepository.GetVentasHoyAsync();

    public async Task<IEnumerable<Venta>> GetVentasPorRangoAsync(DateTime desde, DateTime hasta)
        => await _ventaRepository.GetVentasPorRangoAsync(desde, hasta);

    public async Task<IEnumerable<(string Cliente, decimal Total, int Transacciones)>> GetVentasPorClienteAsync()
        => await _ventaRepository.GetVentasPorClienteAsync();
}