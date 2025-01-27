using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
       
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<ServiceRender> ServiceRenderred { get; set; }
        public DbSet<AreaCovered> AreaCovered { get; set; }
       
    } 
}   
