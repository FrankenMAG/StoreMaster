using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreMaster.Core.Entities;
using StoreMaster.Core.Interfaces;
using StoreMaster.Web.Services;
using StoreMaster.Web.ViewModels;

namespace StoreMaster.Web.Controllers;


[Authorize]
public class ProductosController : Controller
{
    private readonly IProductoService _productoService;
    private readonly ICategoriaService _categoriaService;
    private readonly IProveedorService _proveedorService;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public ProductosController(
        IProductoService productoService,
        ICategoriaService categoriaService,
        IProveedorService proveedorService,
        IFileService fileService,
        IMapper mapper)
    {
        _productoService = productoService;
        _categoriaService = categoriaService;
        _proveedorService = proveedorService;
        _fileService = fileService;
        _mapper = mapper;
    }

    // ── Método privado para llenar los dropdowns ─────────
    private async Task LlenarDropdowns(ProductoViewModel viewModel)
    {
        var categorias = await _categoriaService.GetActivasAsync();
        var proveedores = await _proveedorService.GetActivosAsync();

        viewModel.Categorias = categorias.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Nombre
        });

        viewModel.Proveedores = proveedores.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = p.Nombre
        });
    }

    // ── GET /Productos ───────────────────────────────────
    public async Task<IActionResult> Index()
    {
        var productos = await _productoService.GetAllAsync();
        var viewModels = _mapper.Map<IEnumerable<ProductoViewModel>>(productos);
        return View(viewModels);
    }

    // ── GET /Productos/Create ────────────────────────────
    public async Task<IActionResult> Create()
    {
        var viewModel = new ProductoViewModel();
        await LlenarDropdowns(viewModel);
        return View(viewModel);
    }

    // ── POST /Productos/Create ───────────────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductoViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            await LlenarDropdowns(viewModel);
            return View(viewModel);
        }

        var producto = _mapper.Map<Producto>(viewModel);
        // Procesar imagen si se subió una
        if (viewModel.ImagenFile != null && viewModel.ImagenFile.Length > 0)
        {
            var (success, message, fileName) = await _fileService
                .SaveImageAsync(viewModel.ImagenFile, "productos");

            if (!success)
            {
                ModelState.AddModelError("ImagenFile", message);
                await LlenarDropdowns(viewModel);
                return View(viewModel);
            }

            producto.ImagenUrl = fileName;
        }

        var (ok, msg) = await _productoService.CreateAsync(producto);

        if (ok)
        {
            TempData["Success"] = msg;
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", msg);
        await LlenarDropdowns(viewModel);
        return View(viewModel);
    }

    // ── GET /Productos/Edit/5 ────────────────────────────
    public async Task<IActionResult> Edit(int id)
    {
        var producto = await _productoService.GetByIdAsync(id);
        if (producto == null)
            return NotFound();

        var viewModel = _mapper.Map<ProductoViewModel>(producto);
        await LlenarDropdowns(viewModel);
        return View(viewModel);
    }

    // ── POST /Productos/Edit/5 ───────────────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductoViewModel viewModel)
    {
        if (id != viewModel.Id)
            return BadRequest();

        if (!ModelState.IsValid)
        {
            await LlenarDropdowns(viewModel);
            return View(viewModel);
        }

        var producto = _mapper.Map<Producto>(viewModel);

        // Procesar nueva imagen si se subió una
        if (viewModel.ImagenFile != null && viewModel.ImagenFile.Length > 0)
        {
            // Eliminar imagen anterior
            _fileService.DeleteImage(viewModel.ImagenUrl, "productos");

            var (success, message, fileName) = await _fileService
                .SaveImageAsync(viewModel.ImagenFile, "productos");

            if (!success)
            {
                ModelState.AddModelError("ImagenFile", message);
                await LlenarDropdowns(viewModel);
                return View(viewModel);
            }

            producto.ImagenUrl = fileName;
        }
        else
        {
            // Mantener imagen existente si no se subió una nueva
            producto.ImagenUrl = viewModel.ImagenUrl;
        }

        var (ok, msg) = await _productoService.UpdateAsync(producto);

        if (ok)
        {
            TempData["Success"] = msg;
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", msg);
        await LlenarDropdowns(viewModel);
        return View(viewModel);
    }

    // ── POST /Productos/Delete/5 ─────────────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, message) = await _productoService.DeleteAsync(id);

        if (success)
            TempData["Success"] = message;
        else
            TempData["Error"] = message;

        return RedirectToAction(nameof(Index));
    }
}