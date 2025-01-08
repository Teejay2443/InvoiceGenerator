using InvoiceGenerator.Interface.IRepositories;
using InvoiceGenerator.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator.Repository
{
    public class AreaCoveredRepository : IAreaCoveredRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AreaCoveredRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<AreaCovered>> GetAllAreaCoveredAsync()
        {
            return await _dbContext.AreaCovered.ToListAsync();
        }

        public async Task<AreaCovered> GetAreaCoveredAsync(Guid Id)
        {
            return await _dbContext.AreaCovered.Where(x => x.Id == Id).FirstOrDefaultAsync();
        }
    }
}
