using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator.Dto
{
    public class CreateAreaCoveredDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage ="Name is Required")]
        public string Name { get; set; }
    }
}
