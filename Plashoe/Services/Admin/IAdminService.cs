using Plashoe.DTOs.Products;
using Plashoe.DTOs;
using Plashoe.Model;

namespace Plashoe.Services.Admin
{
    public interface IAdminService
    {
        Task<ApiResponse<object>> EditProduct(int productId, ProductViewDTO updatedProductDto);
        Task<ApiResponse<object>> AddProduct(ProductViewDTO productViewDTO);
        Task<ApiResponse<object>> DeleteProduct(int id);
        Task<ApiResponse<List<OrderDTO>>> GetAllOrders();
        Task<ApiResponse<string>> UpdateOrderStaus(int orderId, string status);
    }

}
