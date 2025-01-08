using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceGenerator.Dto
{
    public class InvoiceDto
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string BuyerName { get; set; }
        //public ServiceRenderDto ServiceRenderDto { get; set; }
        //public AreaCoveredDto areaCoveredDto { get; set; }
        public List<string> ServiceRenderred { get; set; }
        public List<string> AreaOfCoverage { get; set; }
        public string ServiceStartDate { get; set; }
        public string ServiceEndDate { get; set; }

        [Column(TypeName = "Money")]
        public int TotalCost { get; set; }
    }
}
