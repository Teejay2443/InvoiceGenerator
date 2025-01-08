using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator.Dto
{
    public class UpdateServiceRenderDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
