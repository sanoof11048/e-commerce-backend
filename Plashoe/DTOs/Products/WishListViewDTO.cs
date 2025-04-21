using Plashoe.Model;

namespace Plashoe.DTOs.Products
{
    public class WishListViewDTO
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public virtual Product? Products { get; set; }
    }
}
