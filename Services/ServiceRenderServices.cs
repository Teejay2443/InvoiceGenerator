using AspNetCoreHero.ToastNotification.Abstractions;
using InvoiceGenerator.Dto;
using InvoiceGenerator.Interface.IRepositories;
using InvoiceGenerator.Interface.IServices;
using InvoiceGenerator.Models;
using InvoiceGenerator.Repository;
using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator.Services
{
    public class ServiceRenderServices : IServiceRenderServices
    {
        private readonly IServiceRenderRepository _ServiceRenderRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly INotyfService _notyfService;
        public ServiceRenderServices(IServiceRenderRepository ServiceRenderRepository, ApplicationDbContext dbContext, INotyfService notyfService)
        {
            _ServiceRenderRepository = ServiceRenderRepository;
            _dbContext = dbContext;
            _notyfService = notyfService;
        }
        public async Task<BaseResponse<bool>> CreateService(CreateServiceRenderDto request)
        {
            try
            {
                var newService = new ServiceRender()
                {
                    Id = request.Id,
                    Name = request.Name,
                };
                await _dbContext.ServiceRenderred.AddAsync(newService);
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    _notyfService.Success("Service created sucessfully");
                    return new BaseResponse<bool> { Message = "Service created successfully", IsSuccessful = true, Data = true };
                }
                _notyfService.Error("Service Creation failed");
                return new BaseResponse<bool> { Message = "Service Creation failed", IsSuccessful = false, Data = false };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool> { Message = $"Error :  {ex.Message}", IsSuccessful = false, Data = false };
            }
        }

        public async  Task<BaseResponse<bool>> Delete(Guid Id)
        {
            try
            {
                var service = await _ServiceRenderRepository.GetServicebyIdAsync(Id);

                if (service != null)
                {
                    _dbContext.ServiceRenderred.Remove(service);

                    if (await _dbContext.SaveChangesAsync() > 0)
                    {
                        _notyfService.Success("Service deleted sucessfully");
                        return new BaseResponse<bool> { Message = "Service deleted successfully", IsSuccessful = true, Data = true };
                    }

                }
                _notyfService.Error("Service not found");
                return new BaseResponse<bool> { Message = "Service not found", IsSuccessful = false, Data = false };
            }
            catch (Exception ex)
            {

                return new BaseResponse<bool> { Message = $"Error :  {ex.Message}", IsSuccessful = false, Data = false };
            }
        }

        public async Task<BaseResponse<List<ServiceRenderDto>>> GetAllService()
        {
            try
            {
                var Invoices = await _ServiceRenderRepository.GetAllServiceAsync();

                if (Invoices.Count > 0)
                {
                    var data = Invoices.Select(x => new ServiceRenderDto
                    {
                        Id = x.Id,
                        Name = x.Name,


                    }).ToList();

                    return new BaseResponse<List<ServiceRenderDto>> { Message = "Data retrieved successfuly", IsSuccessful = true, Data = data };
                }

                return new BaseResponse<List<ServiceRenderDto>> { Message = "No record", IsSuccessful = false, Data = new List<ServiceRenderDto>() };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ServiceRenderDto>> { Message = $"Error :  {ex.Message}", IsSuccessful = false, Data = new List<ServiceRenderDto>() };
            }
        }

        public async Task<BaseResponse<ServiceRenderDto>> GetService(Guid Id)
        {
            try
            {
                var service = await _ServiceRenderRepository.GetServicebyIdAsync(Id);

                if (service != null)
                {
                    var data = new ServiceRenderDto
                    {
                        Id = service.Id,
                        Name = service.Name,

                    };
                    return new BaseResponse<ServiceRenderDto> { Message = "Data retrieved successfully", IsSuccessful = true, Data = data };
                }

                return new BaseResponse<ServiceRenderDto> { Message = "No record", IsSuccessful = false, Data = new ServiceRenderDto() };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ServiceRenderDto> { Message = $"Error :  {ex.Message}", IsSuccessful = false, Data = new ServiceRenderDto() };
            }
        }

        public Task<BaseResponse<ServiceRender>> GetServiceById(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<bool>> UpdateService(Guid Id, UpdateServiceRenderDto request)
        {
            try
            {
                var service = await _ServiceRenderRepository.GetServicebyIdAsync(Id);

                if (service != null)
                {
                    service.Name = request.Name;


                    _dbContext.ServiceRenderred.Update(service);

                    if (await _dbContext.SaveChangesAsync() > 0)
                    {
                        _notyfService.Success("service updated sucessfully");
                        return new BaseResponse<bool> { Message = "service updated successfully", IsSuccessful = true, Data = true };
                    }
                }
                _notyfService.Error("service not found");
                return new BaseResponse<bool> { Message = "service not found", IsSuccessful = false, Data = false };
            }
            catch (Exception ex)
            {

                return new BaseResponse<bool> { Message = $"Error : {ex.Message}", IsSuccessful = false, Data = false };
            }
        }
    }
}
