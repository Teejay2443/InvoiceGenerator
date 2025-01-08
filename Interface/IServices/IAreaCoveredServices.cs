using InvoiceGenerator.Dto;
using InvoiceGenerator.Models;

namespace InvoiceGenerator.Interface.IServices
{
    public interface IAreaCoveredServices
    {
        Task<BaseResponse<bool>> CreateAreaCovered(CreateAreaCoveredDto request);
        Task<BaseResponse<bool>> UpdateAreaCovered(Guid Id, UpdateAreaCoveredDto request);
        Task<BaseResponse<bool>> Delete(Guid Id);
        Task<BaseResponse<AreaCoveredDto>> GetAreaCovered(Guid Id);
        Task<BaseResponse<AreaCovered>> GetAreaCoveredById(Guid Id);
        Task<BaseResponse<List<AreaCoveredDto>>> GetAllAreaCovered();
    }
}
