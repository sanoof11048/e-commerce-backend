namespace Plashoe.DTOs.Products
{
    public class CartItemDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
        public string? Image { get; set; }
    }
}
