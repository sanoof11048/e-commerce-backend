using System.ComponentModel.DataAnnotations;

namespace Plashoe.DTOs.Products
{
    public class ProductViewDTO
    {
        public int? ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int Stock { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
