using AspNetCoreHero.ToastNotification.Abstractions;
using InvoiceGenerator.Dto;
using InvoiceGenerator.Interface.IRepositories;
using InvoiceGenerator.Interface.IServices;
using InvoiceGenerator.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator.Services
{
    public class InvoiceServices : IInvoiceServices
    {
        private readonly IInvoiceRepository _InvoiceRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly INotyfService _notyfService;
        public InvoiceServices(IInvoiceRepository InvoiceRepository, ApplicationDbContext dbContext, INotyfService notyfService)
        {
            _InvoiceRepository = InvoiceRepository;
            _dbContext = dbContext;
            _notyfService = notyfService;
        }

        public async Task<BaseResponse<bool>> CreateInvoice(CreateInvoiceDto request)
        {
            try
            {
                var newInvoice = new Invoice()
                {
                    Id = request.Id,
                    BuyerName = request.BuyerName,
                    Address = request.Address,
                    AreaOfCoverage = request.AreaOfCoverage,
                    ServiceRenderred = request.ServiceRenderred,
                    ServiceStartDate = request.ServiceStartDate,
                    ServiceEndDate = request.ServiceEndDate,
                    TotalCost = request.TotalCost,
                };
                await _dbContext.Invoices.AddAsync(newInvoice);
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    _notyfService.Success("Invoice created sucessfully");
                    return new BaseResponse<bool> 
                    { 
                        Message = "Invoice created successfully", 
                        IsSuccessful = true, 
                        Data = true 
                    };
                }
                _notyfService.Error("Invoice Creation failed");
                return new BaseResponse<bool> { Message = "Invoice Creation failed", IsSuccessful = false, Data = false };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool> { Message = $"Error :  {ex.Message}", IsSuccessful = false, Data = false };
            }
      
        }

        public async Task<BaseResponse<bool>> Delete(Guid Id)
        {
            try
            {
                var invoice = await _InvoiceRepository.GetInvoiceAsync(Id);

                if (invoice != null)
                {
                    _dbContext.Invoices.Remove(invoice);

                    if (await _dbContext.SaveChangesAsync() > 0)
                    {
                        _notyfService.Success("Invoice deleted sucessfully");
                        return new BaseResponse<bool> { Message = "Invoice deleted successfully", IsSuccessful = true, Data = true };
                    }

                }
                _notyfService.Error("Invoice not found");
                return new BaseResponse<bool> { Message = "Invoice not found", IsSuccessful = false, Data = false };
            }
            catch (Exception ex)
            {

                return new BaseResponse<bool> { Message = $"Error :  {ex.Message}", IsSuccessful = false, Data = false };
            }
        }

        public async Task<BaseResponse<List<InvoiceDto>>> GetAllInvoice()
        {
            try
            {
                var Invoices = await _InvoiceRepository.GetAllInvoicesAsync();

                if (Invoices.Count > 0)
                {
                    var data = Invoices.Select(x => new InvoiceDto
                    {
                        Id = x.Id,
                        BuyerName = x.BuyerName,
                        Address = x.Address,
                        AreaOfCoverage = x.AreaOfCoverage,
                        ServiceRenderred = x.ServiceRenderred,
                        ServiceStartDate = x.ServiceStartDate,
                        ServiceEndDate = x.ServiceEndDate,
                        TotalCost = x.TotalCost,

                    }).ToList();

                    return new BaseResponse<List<InvoiceDto>> { Message = "Data retrieved successfuly", IsSuccessful = true, Data = data };
                }

                return new BaseResponse<List<InvoiceDto>> { Message = "No record", IsSuccessful = false, Data = new List<InvoiceDto>() };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<InvoiceDto>> { Message = $"Error :  {ex.Message}", IsSuccessful = false, Data = new List<InvoiceDto>() };
            }
        }

        public async Task<BaseResponse<InvoiceDto>> GetInvoice(Guid Id)
        {
            try
            {
                var invoice = await _InvoiceRepository.GetInvoiceAsync(Id);

                if (invoice != null)
                {
                    var data = new InvoiceDto
                    {
                        Id = invoice.Id,
                        BuyerName = invoice.BuyerName,
                        Address = invoice.Address,
                        AreaOfCoverage = invoice.AreaOfCoverage,
                        ServiceRenderred = invoice.ServiceRenderred,
                        ServiceStartDate = invoice.ServiceStartDate,
                        ServiceEndDate = invoice.ServiceEndDate,
                        TotalCost = invoice.TotalCost,
                    };
                    return new BaseResponse<InvoiceDto> { Message = "Data retrieved successfully", IsSuccessful = true, Data = data };
                }

                return new BaseResponse<InvoiceDto> { Message = "No record", IsSuccessful = false, Data = new InvoiceDto() };
            }
            catch (Exception ex)
            {
                return new BaseResponse<InvoiceDto> { Message = $"Error :  {ex.Message}", IsSuccessful = false, Data = new InvoiceDto() };
            }
        }

        public async Task<Invoice> GetInvoiceById(Guid Id)
        {
            return await _dbContext.Invoices.FirstOrDefaultAsync();
        }

        public List<SelectServiceDto> GetServiceSelect()
        {
            var serviceNames = _dbContext.ServiceRenderred.ToList();
            var result = new List<SelectServiceDto>();

            if (serviceNames.Count > 0)
            {
                result = serviceNames.Select(x => new SelectServiceDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList();
            }
            return result;
        }
        public List<SelectAreaDto> GetAreaSelect()
        {
            var areaNames = _dbContext.AreaCovered.ToList();
            var result = new List<SelectAreaDto>();

            if (areaNames.Count > 0)
            {
                result = areaNames.Select(x => new SelectAreaDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList();
            }
            return result;
        }

        public async Task<BaseResponse<bool>> UpdateInvoice(Guid Id, UpdateInvoiceDto request)
        {
            try
            {
                var invoice = await _InvoiceRepository.GetInvoiceAsync(Id);

                if (invoice != null)
                {
                    invoice.BuyerName = request.BuyerName;
                    invoice.Address = request.Address;
                    invoice.AreaOfCoverage = request.AreaOfCoverage;
                    invoice.ServiceRenderred = request.ServiceRenderred;
                    invoice.ServiceStartDate = request.ServiceStartDate;
                    invoice.ServiceEndDate = request.ServiceEndDate;
                    invoice.TotalCost = request.TotalCost;

                    _dbContext.Invoices.Update(invoice);

                    if (await _dbContext.SaveChangesAsync() > 0) 
                    {
                        _notyfService.Success("Invoice updated sucessfully");
                        return new BaseResponse<bool> { Message = "Invoice updated successfully", IsSuccessful = true, Data = true };
                    }
                }
                _notyfService.Error("Invoice not found");
                return new BaseResponse<bool> { Message = "Invoice not found", IsSuccessful = false, Data = false };
            }
            catch (Exception ex)
            {

                return new BaseResponse<bool> { Message = $"Error : {ex.Message}", IsSuccessful = false, Data = false };
            }
        }
    }
}
