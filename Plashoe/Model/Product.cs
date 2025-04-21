using System.ComponentModel.DataAnnotations;

namespace Plashoe.Model
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        [Required]
        public decimal? Price { get; set; }
        public string? Image { get; set; }
        public int Stock { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<CartItems>? CartItems { get; set; }
    }
}
