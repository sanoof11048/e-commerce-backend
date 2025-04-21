using Plashoe.DTOs.Products;
using Plashoe.Model;

public interface ICartsService
{
    Task<ApiResponse<string>> AddToCart(int userId, int productId);
    Task<ApiResponse<string>> RemoveCartItem(int userId, int productId);
    Task<ApiResponse<string>> IncreaseQuantity(int userId, int productId);
    Task<ApiResponse<string>> DecreaseQuantity(int userId, int productId);
    Task<ApiResponse<CartViewDTO>> GetCartDetails(int userId);


}
