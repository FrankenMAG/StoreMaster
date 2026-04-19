using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreMaster.Core.Entities;
using StoreMaster.Core.Interfaces;
using StoreMaster.Web.ViewModels;

namespace StoreMaster.Web.Controllers;

[Authorize]
public class ClientesController : Controller
{
    private readonly IClienteService _service;
    private readonly IMapper _mapper;

    public ClientesController(IClienteService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var clientes = await _service.GetAllAsync();
        var viewModels = _mapper.Map<IEnumerable<ClienteViewModel>>(clientes);
        return View(viewModels);
    }

    public IActionResult Create()
        => View(new ClienteViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClienteViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        var cliente = _mapper.Map<Cliente>(viewModel);
        var (success, message) = await _service.CreateAsync(cliente);

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
        var cliente = await _service.GetByIdAsync(id);
        if (cliente == null)
            return NotFound();

        return View(_mapper.Map<ClienteViewModel>(cliente));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ClienteViewModel viewModel)
    {
        if (id != viewModel.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(viewModel);

        var cliente = _mapper.Map<Cliente>(viewModel);
        var (success, message) = await _service.UpdateAsync(cliente);

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