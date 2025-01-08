using InvoiceGenerator.Dto;
using InvoiceGenerator.Interface.IRepositories;
using InvoiceGenerator.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator.Repository
{
    public class ServiceRenderRepository : IServiceRenderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ServiceRenderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<ServiceRender>> GetAllServiceAsync()
        {
            return await _dbContext.ServiceRenderred.ToListAsync();
        }

        public async Task<ServiceRender> GetServicebyIdAsync(Guid Id)
        {
            return await _dbContext.ServiceRenderred.Where(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<List<string>> GetServiceNamesByIdsAsync(List<Guid> serviceRendered)
        {
            // Query the database for services that match the provided IDs
            var serviceNames = await _dbContext.ServiceRenderred
                                               .Where(s => serviceRendered.Contains(s.Id))
                                               .Select(s => s.Name)
                                               .ToListAsync();

            return serviceNames;
        }

    }
}
