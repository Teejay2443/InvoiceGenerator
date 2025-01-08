namespace InvoiceGenerator.Models
{
    public class Auditability
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
    }
}
