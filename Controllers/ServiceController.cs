using AspNetCoreHero.ToastNotification.Abstractions;
using InvoiceGenerator.Dto;
using InvoiceGenerator.Interface.IServices;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IServiceRenderServices _ServiceRenderServices;
        private readonly INotyfService _notyfService;

        public ServiceController(IServiceRenderServices ServiceRenderServices, INotyfService notyfService)
        {
            _ServiceRenderServices = ServiceRenderServices;
            _notyfService = notyfService;
        }
        [HttpGet("all-services")]
        public async Task<IActionResult> Services()
        {
            var result = await _ServiceRenderServices.GetAllService();
            return View(result.Data);
        }
        [HttpGet("create-service")]
        public async Task<IActionResult> CreateService()
        {
            return View();
        }

        [HttpPost("create-service")]
        public async Task<IActionResult> CreateServiceAsync(CreateServiceRenderDto request)
        {
            var result = await _ServiceRenderServices.CreateService(request);
            if (result.IsSuccessful)
            {
                _notyfService.Success("Service created sucessfully");
                return RedirectToAction("Services");
            }
            _notyfService.Error("Service created unsucessfully");
            return RedirectToAction("Services");
        }
        [HttpGet("delete-service/{Id}")]
        public async Task<IActionResult> DeleteServiceAsync([FromRoute] Guid Id)
        {
            var result = await _ServiceRenderServices.Delete(Id);
            if (result.IsSuccessful)
            {
                _notyfService.Success("Service deleted sucessfully");
                return RedirectToAction("Services");
            }
            _notyfService.Success("Service not deleted sucessfully");
            return RedirectToAction("Services");
        }
        [HttpGet("Service/{Id}")]
        public async Task<IActionResult> ServiceDetail([FromRoute] Guid Id)
        {
            var result = await _ServiceRenderServices.GetService(Id);
            return View(result.Data);
        }
        [HttpGet("update-service/{Id}")]
        public async Task<IActionResult> UpdateService([FromRoute] Guid Id)
        {
            var result = await _ServiceRenderServices.GetService(Id);
            return View(result.Data);
        }
        [HttpPost("update-service/{Id}")]
        public async Task<IActionResult> UpdateServiceAsync([FromRoute] Guid Id, [FromForm] UpdateServiceRenderDto request)
        {
            var result = await _ServiceRenderServices.UpdateService(Id, request);
            if (result.IsSuccessful)
            {
                _notyfService.Success("Service Updated sucessfully");
                return RedirectToAction("ServiceDetail", new { Id = Id });
            }

            _notyfService.Error("Service not updated sucessfully");
            return RedirectToAction("Services");
        }
    }
}
