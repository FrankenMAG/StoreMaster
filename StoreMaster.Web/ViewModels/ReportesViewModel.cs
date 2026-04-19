using StoreMaster.Core.Entities;

namespace StoreMaster.Web.ViewModels;

public class ReportesViewModel
{
    // Filtros
    public DateTime Desde { get; set; } = DateTime.Today.AddDays(-30);
    public DateTime Hasta { get; set; } = DateTime.Today;

    // Reporte ventas por rango
    public List<Venta> VentasPorRango { get; set; } = [];
    public decimal TotalRango { get; set; }
    public int CountRango { get; set; }

    // Reporte inventario
    public List<Producto> Inventario { get; set; } = [];
    public decimal ValorTotalInventario { get; set; }

    // Reporte ventas por cliente
    public List<(string Cliente, decimal Total, int Transacciones)> VentasPorCliente { get; set; } = [];
}