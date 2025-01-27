using InvoiceGenerator.Dto;
using InvoiceGenerator.Models;

namespace InvoiceGenerator.Interface.IServices
{
    public interface IInvoiceServices
    {
        Task<BaseResponse<bool>> CreateInvoice(CreateInvoiceDto request);
        Task<BaseResponse<bool>> UpdateInvoice(Guid Id, UpdateInvoiceDto request);
        Task<BaseResponse<bool>> Delete(Guid Id);
        Task<BaseResponse<InvoiceDto>> GetInvoice(Guid Id);
        Task<Invoice> GetInvoiceById(Guid Id);
        Task<BaseResponse<List<InvoiceDto>>> GetAllInvoice();
        List<SelectServiceDto> GetServiceSelect();
        List<SelectAreaDto> GetAreaSelect();


    }
}
