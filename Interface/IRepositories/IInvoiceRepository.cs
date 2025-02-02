using InvoiceGenerator.Models;

namespace InvoiceGenerator.Interface.IRepositories
{
    public interface IInvoiceRepository
    {
        Task<Invoice> GetInvoiceAsync(Guid Id);
        Task<List<Invoice>> GetAllInvoicesAsync();
        Task<Invoice> GetInvoiceById(Guid Id);
    }
}
