using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreMaster.Core.Interfaces;
using StoreMaster.Web.ViewModels;

namespace StoreMaster.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IVentaService _ventaService;
    private readonly IProductoService _productoService;
    private readonly IClienteService _clienteService;

    public HomeController(
        IVentaService ventaService,
        IProductoService productoService,
        IClienteService clienteService)
    {
        _ventaService = ventaService;
        _productoService = productoService;
        _clienteService = clienteService;
    }

    public async Task<IActionResult> Index()
    {
        var ventasHoy = await _ventaService.GetVentasHoyAsync();
        var ventasUltimos7Dias = await _ventaService.GetVentasUltimosDiasAsync(7);
        var stockBajo = await _productoService.GetStockBajoAsync();
        var topProductos = await _productoService.GetTopProductosAsync(5);

        // Construir datos para gráfica de los últimos 7 días
        var labels = new List<string>();
        var dataVentas = new List<decimal>();

        for (int i = 6; i >= 0; i--)
        {
            var fecha = DateTime.UtcNow.Date.AddDays(-i);
            labels.Add(fecha.ToString("dd/MM"));

            var totalDia = ventasUltimos7Dias
                .Where(v => v.Fecha.Date == fecha)
                .Sum(v => v.Total);

            dataVentas.Add(totalDia);
        }

        var viewModel = new DashboardViewModel
        {
            TotalVentasHoy = ventasHoy.Sum(v => v.Total),
            TotalVentasHoyCount = ventasHoy.Count(),
            TotalProductos = await _productoService.GetTotalActivosAsync(),
            TotalClientes = await _clienteService.GetTotalActivosAsync(),
            TotalStockBajo = stockBajo.Count(),
            Labels = labels,
            DataVentas = dataVentas,
            TopProductos = topProductos.ToList(),
            UltimasVentas = ventasHoy.Take(5).ToList(),
            ProductosStockBajo = stockBajo.Take(5).ToList()
        };

        return View(viewModel);
    }
}