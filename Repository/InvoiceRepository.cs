using InvoiceGenerator.Interface.IRepositories;
using InvoiceGenerator.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public InvoiceRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Invoice>> GetAllInvoicesAsync()
        {
            return await _dbContext.Invoices
                .ToListAsync();
        }

        public async Task<Invoice> GetInvoiceAsync(Guid Id)
        {
            return await _dbContext.Invoices.Where(x => x.Id == Id).FirstOrDefaultAsync();
        }


    }
}
