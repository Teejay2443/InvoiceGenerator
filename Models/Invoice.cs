using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceGenerator.Models
{
    public class Invoice : Auditability
    {
        public string BuyerName { get; set; }
        public string Address { get; set; }
        public List<string> ServiceRenderred  { get; set; }
        public List<string> AreaOfCoverage { get; set; }
        public string ServiceStartDate { get; set; }
        public string ServiceEndDate { get; set; }

        [Column(TypeName = "Money")]
        public int TotalCost { get; set; }

    }
}
