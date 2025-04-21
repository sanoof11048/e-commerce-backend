namespace Plashoe.DTOs.Products
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }  // Add this
    }

}
