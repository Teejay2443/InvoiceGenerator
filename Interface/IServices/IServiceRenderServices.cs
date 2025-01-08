using InvoiceGenerator.Dto;
using InvoiceGenerator.Models;

namespace InvoiceGenerator.Interface.IServices
{
    public interface IServiceRenderServices
    {
        Task<BaseResponse<bool>> CreateService(CreateServiceRenderDto request);
        Task<BaseResponse<bool>> UpdateService(Guid Id, UpdateServiceRenderDto request);
        Task<BaseResponse<bool>> Delete(Guid Id);
        Task<BaseResponse<ServiceRenderDto>> GetService(Guid Id);
        Task<BaseResponse<ServiceRender>> GetServiceById(Guid Id);
        Task<BaseResponse<List<ServiceRenderDto>>> GetAllService();
    }
}
