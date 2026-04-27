using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreMaster.Core.Entities;
using StoreMaster.Core.Interfaces;
using StoreMaster.Web.ViewModels;

namespace StoreMaster.Web.Controllers
{
    [Authorize]
    public class FacturasController : Controller
    {
        private readonly IFacturaService _facturaService;
        private readonly IMapper _mapper;

        public FacturasController(IFacturaService facturaService, IMapper mapper)
        {
            _facturaService = facturaService;
            _mapper = mapper;
        }


        // GET: FacturaController
        public async Task<ActionResult> Index()
        {
           var facturas= await _facturaService.GetAllAsync();
            var viewModels= _mapper.Map<IEnumerable<FacturaViewModel>>(facturas);
            return View(viewModels);
        }

        // GET: FacturaController/Detalle/5
        public async Task<IActionResult> Detalle(int id)
        {
            var factura = await _facturaService.GetByIdAsync(id);
            if (factura == null)
                return NotFound();

            var viewModel = _mapper.Map<FacturaViewModel>(factura);
            return View(viewModel);
        }

        // GET: FacturaController/Create
        public IActionResult Create(int ventaId)
        {
            var viewModel = new CrearFacturaViewModel
            {
                VentaId = ventaId,
                Serie = "A"
            };
            return View(viewModel);
        }

        // POST: FacturaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearFacturaViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var (sucess,message,facturaId) = await _facturaService
                .CrearFacturaAsync(viewModel.VentaId, viewModel.Serie);

            if (sucess)
            {
                TempData["Success"]=message;
                return RedirectToAction(nameof(Detalle), new { id = facturaId });
            }
            ModelState.AddModelError("", message);
            return View(viewModel); 
        }

        public async Task<IActionResult> Cancelar(int id)
        {
            var (success, message) = await _facturaService.CancelarFacturaAsync(id);
            if (success) 
                TempData["Success"] = message;
            else
                TempData["Error"] = message;

            return RedirectToAction(nameof(Detalle),new { id });
        }
    }
}
