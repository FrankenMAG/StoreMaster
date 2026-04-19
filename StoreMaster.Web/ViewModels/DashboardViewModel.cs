namespace StoreMaster.Web.ViewModels;

public class DashboardViewModel
{
    // KPIs
    public decimal TotalVentasHoy { get; set; }
    public int TotalVentasHoyCount { get; set; }
    public int TotalProductos { get; set; }
    public int TotalClientes { get; set; }
    public int TotalStockBajo { get; set; }

    // Gráfica de ventas últimos 7 días
    public List<string> Labels { get; set; } = [];
    public List<decimal> DataVentas { get; set; } = [];

    // Top productos
    public List<(string Nombre, int Total)> TopProductos { get; set; } = [];

    // Últimas ventas del día
    public List<StoreMaster.Core.Entities.Venta> UltimasVentas { get; set; } = [];

    // Productos con stock bajo
    public List<StoreMaster.Core.Entities.Producto> ProductosStockBajo { get; set; } = [];
}