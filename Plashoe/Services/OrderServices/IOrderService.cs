using Plashoe.DTOs.Products;
using Plashoe.Model;

namespace Plashoe.Services.OrderServices
{
    public interface IOrderService
    {
        Task<ApiResponse<List<OrderDTO>>> GetOrders(int userId);
        Task<ApiResponse<string>> PlaceOrder(OrderCreateDTO order);
        Task<ApiResponse<string>> CancelOrder(int orderId);
    }
}
