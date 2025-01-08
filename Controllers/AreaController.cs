using AspNetCoreHero.ToastNotification.Abstractions;
using InvoiceGenerator.Dto;
using InvoiceGenerator.Interface.IServices;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator.Controllers
{
    public class AreaController : Controller
    {
        private readonly IAreaCoveredServices _AreaCoveredServices;
        private readonly INotyfService _notyfService;

        public AreaController(IAreaCoveredServices AreaCoveredServices, INotyfService notyfService)
        {
            _AreaCoveredServices = AreaCoveredServices;
            _notyfService = notyfService;
        }
        [HttpGet("all-area")]
        public async Task<IActionResult> Areas()
        {
            var result = await _AreaCoveredServices.GetAllAreaCovered();
            return View(result.Data);
        }
        [HttpGet("create-area")]
        public async Task<IActionResult> CreateArea()
        {
            return View();
        }

        [HttpPost("create-area")]
        public async Task<IActionResult> CreateAreaAsync(CreateAreaCoveredDto request)
        {
            var result = await _AreaCoveredServices.CreateAreaCovered(request);
            if (result.IsSuccessful)
            {
                _notyfService.Success("AreaCovered created sucessfully");
                return RedirectToAction("Areas");
            }
            _notyfService.Error("AreaCovered created unsucessfully");
            return RedirectToAction("Areas");
        }
        [HttpGet("delete-area/{Id}")]
        public async Task<IActionResult> DeleteAreaAsync([FromRoute] Guid Id)
        {
            var result = await _AreaCoveredServices.Delete(Id);
            if (result.IsSuccessful)
            {
                _notyfService.Success("Area Covered deleted sucessfully");
                return RedirectToAction("Areas");
            }
            _notyfService.Success("Area Covered not deleted sucessfully");
            return RedirectToAction("Areas");
        }
        [HttpGet("area-detail/{Id}")]
        public async Task<IActionResult> AreaDetail([FromRoute] Guid Id)
        {
            var result = await _AreaCoveredServices.GetAreaCovered(Id);
            return View(result.Data);
        }
        [HttpGet("update-area/{Id}")]
        public async Task<IActionResult> UpdateArea([FromRoute] Guid Id)
        {
            var result = await _AreaCoveredServices.GetAreaCovered(Id);
            return View(result.Data);
        }
        [HttpPost("update-area/{Id}")]
        public async Task<IActionResult> UpdateArea([FromRoute] Guid Id, [FromForm] UpdateAreaCoveredDto request)
        {
            var result = await _AreaCoveredServices.UpdateAreaCovered(Id, request);
            if (result.IsSuccessful)
            {
                _notyfService.Success("Area Covered Updated sucessfully");
                return RedirectToAction("Areas", new { Id = Id });
            }

            _notyfService.Error("Area Covered not updated sucessfully");
            return RedirectToAction("Areas");
        }

    }
}
