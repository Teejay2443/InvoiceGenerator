using InvoiceGenerator.Models;

namespace InvoiceGenerator.Interface.IRepositories
{
    public interface IAreaCoveredRepository
    {

        Task<AreaCovered> GetAreaCoveredAsync(Guid Id);
        Task<List<AreaCovered>> GetAllAreaCoveredAsync();
    }
}
