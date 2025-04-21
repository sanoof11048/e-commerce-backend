using Plashoe.DTOs;
using Plashoe.Model;

namespace Plashoe.Services.WishLists
{
    public interface IWishListService
    {
        Task<ApiResponse<object>> AddtoWish(int userId,int ProductId);
        Task<List<Wishlist>> GetWishList(int userId);
        Task<ApiResponse<object>> DeleteWishList(int userId, int productId);
    }
}
