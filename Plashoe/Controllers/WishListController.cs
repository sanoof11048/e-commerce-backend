using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plashoe.DTOs;
using Plashoe.Model;
using Plashoe.Services.WishList;
using Plashoe.Services.WishLists;

namespace Plashoe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishListService;
        public WishListController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddToWishList( int userId, int productId)
        {
            ApiResponse<object> isadded = await _wishListService.AddtoWish(userId, productId);
                Console.WriteLine(isadded.Error);
            if (isadded.Error == null)
            {
                return Ok("The product is added to wishlist");
            }
            else
            {
                return BadRequest("Wrong item or item already in the wishlist");
            }
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteWishlist(int userId, int productId)
        {
            ApiResponse<object> isDelete = await _wishListService.DeleteWishList(userId, productId);

            if (isDelete.Error == null)
            {
                return Ok("The product is deleted from wishlist");
            }
            else
            {
                return BadRequest("Wrong item or item not found in the wishlist");
            }
        }

        [Authorize]
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetWishlist(int Id)
        {
            try
            {
                List<Wishlist> wish = await _wishListService.GetWishList(Id);
                if (wish == null || wish.Count == 0)
                {
                    return Ok(new List<Wishlist>());
                }
                return Ok(wish);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
