using System.ComponentModel.DataAnnotations;

namespace Plashoe.DTOs.Products
{
    public class WishlistDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}
