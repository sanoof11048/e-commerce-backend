namespace Plashoe.DTOs.Products
{
    public class OrderCreateDTO
    {
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
        public string TransactionId { get; set; }
    }
}
