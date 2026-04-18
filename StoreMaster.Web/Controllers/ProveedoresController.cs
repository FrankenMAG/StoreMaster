using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreMaster.Core.Entities;
using StoreMaster.Core.Interfaces;
using StoreMaster.Web.ViewModels;

namespace StoreMaster.Web.Controllers;

[Authorize]
public class ProveedoresController : Controller
{
    private readonly IProveedorService _service;
    private readonly IMapper _mapper;

    public ProveedoresController(IProveedorService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var proveedores = await _service.GetAllAsync();
        var viewModels = _mapper.Map<IEnumerable<ProveedorViewModel>>(proveedores);
        return View(viewModels);
    }

    public IActionResult Create()
        => View(new ProveedorViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProveedorViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        var proveedor = _mapper.Map<Proveedor>(viewModel);
        var (success, message) = await _service.CreateAsync(proveedor);

        if (success)
        {
            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", message);
        return View(viewModel);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var proveedor = await _service.GetByIdAsync(id);
        if (proveedor == null)
            return NotFound();

        return View(_mapper.Map<ProveedorViewModel>(proveedor));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProveedorViewModel viewModel)
    {
        if (id != viewModel.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(viewModel);

        var proveedor = _mapper.Map<Proveedor>(viewModel);
        var (success, message) = await _service.UpdateAsync(proveedor);

        if (success)
        {
            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", message);
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, message) = await _service.DeleteAsync(id);

        if (success)
            TempData["Success"] = message;
        else
            TempData["Error"] = message;

        return RedirectToAction(nameof(Index));
    }
}