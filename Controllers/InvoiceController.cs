using AspNetCoreHero.ToastNotification.Abstractions;
using InvoiceGenerator.Dto;
using InvoiceGenerator.Interface.IRepositories;
using InvoiceGenerator.Interface.IServices;
using InvoiceGenerator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace InvoiceGenerator.Controllers
{
    [Route("invoices")]
    public class InvoiceController : Controller
    {
        private readonly IInvoiceServices _InvoiceServices;
        private readonly IServiceRenderRepository _ServiceRenderRepository;
        private readonly IViewEngine _viewEngine;
        private readonly INotyfService _notyfService;

        public InvoiceController(
            IInvoiceServices InvoiceServices,
            INotyfService notyfService,
            IServiceRenderRepository ServiceRenderRepository
           )
        {
            _InvoiceServices = InvoiceServices;
            _notyfService = notyfService;
            _ServiceRenderRepository = ServiceRenderRepository;
            
        }
        [HttpGet("all-invoices")]
        public async Task<IActionResult> Invoices()
        {
            var result = await _InvoiceServices.GetAllInvoice();
            return View(result.Data);
        }



        [HttpGet("create-invoice")]
        public async Task<IActionResult> CreateInvoice()
        {
            var selectService = _InvoiceServices.GetServiceSelect();
            if (selectService == null)
            {
                selectService = new List<SelectServiceDto>();
            }
            ViewData["SelectService"] = selectService;

            var selectArea = _InvoiceServices.GetAreaSelect();
            if (selectArea == null)
            {
                selectArea = new List<SelectAreaDto>();
            }
            ViewData["SelectArea"] = selectArea;
            return View();
        }
        [HttpPost("create-invoice")]
        public async Task<IActionResult> CreateInvoiceAsync(CreateInvoiceDto request)
        {
           
            var result = await _InvoiceServices.CreateInvoice(request);
            if (result.IsSuccessful)
            {
                return RedirectToAction("Invoices");
            }
            return RedirectToAction("Invoices");
        }
        [HttpGet("delete-invoice/{Id}")]
        public async Task<IActionResult> DeleteInvoiceAsync([FromRoute] Guid Id)
        {
            var result = await _InvoiceServices.Delete(Id);
            if (result.IsSuccessful)
            {
                return RedirectToAction("Invoices");
            }
            return RedirectToAction("Invoices");
        }
        [HttpGet("Invoice/{Id}")]
        public async Task<IActionResult> InvoiceDetail([FromRoute] Guid Id)
        {
            var result = await _InvoiceServices.GetInvoice(Id);
            return View(result.Data);
        }
        [HttpGet("update-invoice/{Id}")]
        public async Task<IActionResult> UpdateInvoice([FromRoute] Guid Id)
        {
            var selectService = _InvoiceServices.GetServiceSelect();
            if (selectService == null)
            {
                selectService = new List<SelectServiceDto>();
            }
            ViewData["SelectService"] = selectService;

            var selectArea = _InvoiceServices.GetAreaSelect();
            if (selectArea == null)
            {
                selectArea = new List<SelectAreaDto>();
            }
            ViewData["SelectArea"] = selectArea;
            var result = await _InvoiceServices.GetInvoice(Id);
            return View(result.Data);
        }
        [HttpPost("update-invoice/{Id}")]
        public async Task<IActionResult> UpdateInvoiceAsync([FromRoute] Guid Id, [FromForm] UpdateInvoiceDto request)
        {
            var result = await _InvoiceServices.UpdateInvoice(Id, request);
            if (result.IsSuccessful)
            {
                return RedirectToAction("InvoiceDetail", new { Id = Id });
            }
            return RedirectToAction("Invoice");
        }
    }
}
