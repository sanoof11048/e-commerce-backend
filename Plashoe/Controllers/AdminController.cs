using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plashoe.DTOs.Products;
using Plashoe.DTOs;
using Plashoe.Services.Admin;
using Plashoe.Model;

namespace Plashoe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("editProduct/{productId:int}")]
        public async Task<ActionResult<ApiResponse<object>>> Edit(int productId, ProductViewDTO updatedProduct)
        {
            updatedProduct.ProductId = productId;
            var result = await _adminService.EditProduct(productId, updatedProduct);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("product-delete/{id:int}")]
        public async Task<ActionResult<ApiResponse<object>>> RemoveProduct(int id)
        {
            var result = await _adminService.DeleteProduct(id);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addProduct")]
        public async Task<ActionResult<ApiResponse<object>>> AddProduct(ProductViewDTO productViewDTO)
        {
            var result = await _adminService.AddProduct(productViewDTO);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("all-orders")]
        public async Task<IActionResult> GetOrders()
        {
            var result = await _adminService.GetAllOrders();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("Order-Status-Update/{id:int}/{status:alpha}")]
        public async Task<IActionResult> ChangeStatus(int id, string status)
        {
            {
                var result = await _adminService.UpdateOrderStaus(id, status);
                return Ok(result);
            }

        }
    }
}
