using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceGenerator.Dto
{
    public class CreateInvoiceDto
    {
        public Guid  Id { get; set; }

        [Required(ErrorMessage = "BuyerName is required")]
        public string BuyerName { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "ServiceRenderred is required")]
        public List<string> ServiceRenderred { get; set; }
        [Required(ErrorMessage = "AreaOfCoverage is required")]
        public List<string> AreaOfCoverage { get; set; }
        [Required(ErrorMessage = "ServiceStartDate is required")]
        public string ServiceStartDate { get; set; }

        [Required(ErrorMessage = "ServiceEndDate is required")]
        public string ServiceEndDate { get; set; }

        [Column(TypeName = "Money")]
        public int TotalCost { get; set; }
    }
}
