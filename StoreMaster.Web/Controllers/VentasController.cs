using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreMaster.Core.Entities;
using StoreMaster.Core.Interfaces;
using StoreMaster.Web.Services;

namespace StoreMaster.Web.Controllers;

[Authorize]
public class VentasController : Controller
{
    private readonly IVentaService _ventaService;
    private readonly IProductoService _productoService;
    private readonly IClienteService _clienteService;
    private readonly CarritoService _carritoService;

    public VentasController(
        IVentaService ventaService,
        IProductoService productoService,
        IClienteService clienteService,
        CarritoService carritoService)
    {
        _ventaService = ventaService;
        _productoService = productoService;
        _clienteService = clienteService;
        _carritoService = carritoService;
    }

    // ── GET /Ventas ──────────────────────────────────────
    public async Task<IActionResult> Index()
    {
        var ventas = await _ventaService.GetAllAsync();
        return View(ventas);
    }

    // ── GET /Ventas/PuntoDeVenta ─────────────────────────
    public async Task<IActionResult> PuntoDeVenta()
    {
        var carrito = _carritoService.ObtenerCarrito();
        var clientes = await _clienteService.GetActivosAsync();

        ViewBag.Clientes = new SelectList(clientes, "Id", "NombreCompleto");
        ViewBag.Carrito = carrito;

        return View();
    }
    // ── GET /Ventas/ObtenerCarritoPartial ────────────────
    public IActionResult ObtenerCarritoPartial()
    {
        var carrito = _carritoService.ObtenerCarrito();
        return PartialView("_CarritoItems", carrito);
    }
    // ── POST /Ventas/BuscarProducto ──────────────────────
    [HttpPost]
    public async Task<IActionResult> BuscarProducto(string termino)
    {
        var productos = await _productoService.GetActivosAsync();
        var resultado = productos
            .Where(p => p.Nombre.Contains(termino, StringComparison.OrdinalIgnoreCase) ||
                        (p.CodigoBarras != null && p.CodigoBarras.Contains(termino)))
            .Take(10)
            .Select(p => new
            {
                p.Id,
                p.Nombre,
                p.Precio,
                p.Stock,
                p.ImagenUrl,
                CategoriaNombre = p.Categoria?.Nombre
            });

        return Json(resultado);
    }

    // ── POST /Ventas/AgregarAlCarrito ────────────────────
    [HttpPost]
    public IActionResult AgregarAlCarrito(int productoId, string nombre,
        decimal precio, int cantidad, string? imagenUrl)
    {
        if (cantidad <= 0)
            return Json(new { success = false, message = "La cantidad debe ser mayor a cero." });

        var item = new CarritoItem
        {
            ProductoId = productoId,
            Nombre = nombre,
            PrecioUnitario = precio,
            Cantidad = cantidad,
            ImagenUrl = imagenUrl
        };

        _carritoService.AgregarItem(item);
        var carrito = _carritoService.ObtenerCarrito();

        return Json(new
        {
            success = true,
            totalItems = carrito.TotalItems,
            total = carrito.Total.ToString("N2")
        });
    }

    // ── POST /Ventas/EliminarDelCarrito ──────────────────
    [HttpPost]
    public IActionResult EliminarDelCarrito(int productoId)
    {
        _carritoService.EliminarItem(productoId);
        var carrito = _carritoService.ObtenerCarrito();

        return Json(new
        {
            success = true,
            totalItems = carrito.TotalItems,
            total = carrito.Total.ToString("N2")
        });
    }

    // ── POST /Ventas/ActualizarCantidad ──────────────────
    [HttpPost]
    public IActionResult ActualizarCantidad(int productoId, int cantidad)
    {
        _carritoService.ActualizarCantidad(productoId, cantidad);
        var carrito = _carritoService.ObtenerCarrito();

        return Json(new
        {
            success = true,
            totalItems = carrito.TotalItems,
            subtotal = carrito.Subtotal.ToString("N2"),
            impuesto = carrito.Impuesto.ToString("N2"),
            total = carrito.Total.ToString("N2")
        });
    }

    // ── POST /Ventas/ConfirmarVenta ──────────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarVenta(int? clienteId)
    {
        var carrito = _carritoService.ObtenerCarrito();
        var (success, message, ventaId) = await _ventaService.CrearVentaAsync(carrito, clienteId);

        if (success)
        {
            _carritoService.LimpiarCarrito();
            TempData["Success"] = message;
            return RedirectToAction(nameof(Detalle), new { id = ventaId });
        }

        TempData["Error"] = message;
        return RedirectToAction(nameof(PuntoDeVenta));
    }

    // ── GET /Ventas/Detalle/5 ────────────────────────────
    public async Task<IActionResult> Detalle(int id)
    {
        var venta = await _ventaService.GetByIdAsync(id);
        if (venta == null)
            return NotFound();

        return View(venta);
    }

    // ── POST /Ventas/Cancelar/5 ──────────────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancelar(int id)
    {
        var (success, message) = await _ventaService.CancelarVentaAsync(id);

        if (success)
            TempData["Success"] = message;
        else
            TempData["Error"] = message;

        return RedirectToAction(nameof(Detalle), new { id });
    }
}