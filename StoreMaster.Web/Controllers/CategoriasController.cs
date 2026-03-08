using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreMaster.Core.Entities;
using StoreMaster.Core.Interfaces;
using StoreMaster.Web.ViewModels;

namespace StoreMaster.Web.Controllers
{
    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly ICategoriaService _service;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriaService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // ── GET /Categorias ──────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var categorias = await _service.GetAllAsync();
            var viewModels = _mapper.Map<IEnumerable<CategoriaViewModel>>(categorias);
            return View(viewModels);
        }

        // ── GET /Categorias/Create ───────────────────────────
        public IActionResult Create()
        {
            return View(new CategoriaViewModel());
        }

        // ── POST /Categorias/Create ──────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriaViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var categoria = _mapper.Map<Categoria>(viewModel);
            var (success, message) = await _service.CreateAsync(categoria);

            if (success)
            {
                TempData["Success"] = message;
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", message);
            return View(viewModel);
        }

        // ── GET /Categorias/Edit/5 ───────────────────────────
        public async Task<IActionResult> Edit(int id)
        {
            var categoria = await _service.GetByIdAsync(id);
            if (categoria == null)
                return NotFound();

            var viewModel = _mapper.Map<CategoriaViewModel>(categoria);
            return View(viewModel);
        }

        // ── POST /Categorias/Edit/5 ──────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoriaViewModel viewModel)
        {
            if (id != viewModel.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(viewModel);

            var categoria = _mapper.Map<Categoria>(viewModel);
            var (success, message) = await _service.UpdateAsync(categoria);

            if (success)
            {
                TempData["Success"] = message;
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", message);
            return View(viewModel);
        }

        // ── POST /Categorias/Delete/5 ────────────────────────
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
}
