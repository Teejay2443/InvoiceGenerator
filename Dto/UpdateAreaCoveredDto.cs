using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator.Dto
{
    public class UpdateAreaCoveredDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
    }
}
