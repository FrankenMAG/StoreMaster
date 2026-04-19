using StoreMaster.Core.Entities;

namespace StoreMaster.Core.Interfaces;

public interface IVentaService
{
    Task<IEnumerable<Venta>> GetAllAsync();
    Task<Venta?> GetByIdAsync(int id);
    Task<IEnumerable<Venta>> GetByFechaAsync(DateTime desde, DateTime hasta);
    Task<(bool Success, string Message, int VentaId)> CrearVentaAsync(
        Carrito carrito, int? clienteId);
    Task<(bool Success, string Message)> CancelarVentaAsync(int id);
    Task<decimal> GetTotalVentasHoyAsync();
    Task<int> GetTotalVentasHoyCountAsync();

    Task<IEnumerable<Venta>> GetVentasUltimosDiasAsync(int dias = 7);
    Task<IEnumerable<Venta>> GetVentasHoyAsync();

    Task<IEnumerable<Venta>> GetVentasPorRangoAsync(DateTime desde, DateTime hasta);
    Task<IEnumerable<(string Cliente, decimal Total, int Transacciones)>> GetVentasPorClienteAsync();
}