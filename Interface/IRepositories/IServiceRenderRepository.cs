using InvoiceGenerator.Dto;
using InvoiceGenerator.Models;

namespace InvoiceGenerator.Interface.IRepositories
{
    public interface IServiceRenderRepository
    {
        Task<List<ServiceRender>> GetAllServiceAsync();
        Task<ServiceRender> GetServicebyIdAsync(Guid Id);
        public  Task<List<string>> GetServiceNamesByIdsAsync(List<Guid> serviceRendered);
    }
}
