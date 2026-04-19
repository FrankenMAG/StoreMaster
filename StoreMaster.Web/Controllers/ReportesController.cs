using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreMaster.Core.Interfaces;
using StoreMaster.Web.ViewModels;

namespace StoreMaster.Web.Controllers;

[Authorize]
public class ReportesController : Controller
{
    private readonly IVentaService _ventaService;
    private readonly IProductoService _productoService;

    public ReportesController(
        IVentaService ventaService,
        IProductoService productoService)
    {
        _ventaService = ventaService;
        _productoService = productoService;
    }

    public async Task<IActionResult> Index(DateTime? desde, DateTime? hasta)
    {
        var fechaDesde = desde ?? DateTime.Today.AddDays(-30);
        var fechaHasta = hasta ?? DateTime.Today;

        var ventasPorRango = await _ventaService
            .GetVentasPorRangoAsync(fechaDesde, fechaHasta);

        var inventario = await _productoService.GetAllAsync();

        var ventasPorCliente = await _ventaService.GetVentasPorClienteAsync();

        var viewModel = new ReportesViewModel
        {
            Desde = fechaDesde,
            Hasta = fechaHasta,
            VentasPorRango = ventasPorRango.ToList(),
            TotalRango = ventasPorRango.Sum(v => v.Total),
            CountRango = ventasPorRango.Count(),
            Inventario = inventario.ToList(),
            ValorTotalInventario = inventario.Sum(p => p.Stock * p.Precio),
            VentasPorCliente = ventasPorCliente.ToList()
        };

        return View(viewModel);
    }
}