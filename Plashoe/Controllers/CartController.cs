using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plashoe.DTOs;
using Plashoe.DTOs.Products;
using Plashoe.Model;
using Plashoe.Services.Carts;

namespace Plashoe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartsService _cartService;

        public CartController(ICartsService cartService)
        {
            _cartService = cartService;
        }
        [Authorize]
        [HttpPost("add/{userId:int}/{productId:int}")]
        public async Task<IActionResult> AddToCart(int userId, int productId)
        {
            var response = await _cartService.AddToCart(userId, productId);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize]
        [HttpDelete("remove/{userId:int}/{productId:int}")]
        public async Task<IActionResult> RemoveCartItem(int userId, int productId)
        {
            var response = await _cartService.RemoveCartItem(userId, productId);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize]
        [HttpPatch("increase/{userId:int}/{productId:int}")]
        public async Task<IActionResult> IncreaseQuantity(int userId, int productId)
        {
            var response = await _cartService.IncreaseQuantity(userId, productId);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize]
        [HttpPatch("decrease/{userId:int}/{productId:int}")]
        public async Task<IActionResult> DecreaseQuantity(int userId, int productId)
        {
            var response = await _cartService.DecreaseQuantity(userId, productId);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize]
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var response = await _cartService.GetCartDetails(userId);
            if (response.StatusCode != 200)
                return StatusCode(response.StatusCode, response);


            return StatusCode(response.StatusCode, response);

        }
    }
}
