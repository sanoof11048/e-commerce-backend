namespace Plashoe.DTOs.Products
{
    public class CartViewDTO
    {
        public int Id { get; set; }
        public List<CartItemDTO> Items { get; set; } = new();
        public decimal TotalPrice { get; set; }
    }
}
