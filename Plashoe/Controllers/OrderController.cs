using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plashoe.DTOs.Products;
using Plashoe.Model;
using Plashoe.Services.OrderServices;

namespace Plashoe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetOrders(int userId)
        {
            var result = await _orderService.GetOrders(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderCreateDTO order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _orderService.PlaceOrder(order);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("cancel/{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var result = await _orderService.CancelOrder(orderId);
            return StatusCode(result.StatusCode, result);
        }
    }

}
