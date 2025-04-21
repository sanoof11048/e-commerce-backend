using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plashoe.DTOs;
using Plashoe.DTOs.Products;
using Plashoe.Model;
using Plashoe.Services.Products;

namespace Plashoe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductBYId(int id)
        {
            var products = await _productService.GetProductById(id);
            if (products == null)
            {
                return NotFound("There is no product exist in this id");
            }
            return Ok(products);
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetbyCategory(string category)
        {
            var products = await _productService.GetProductByCategory(category);
            if (products == null || !products.Any())
            {
                return NotFound("No products found in this category.");
            }
            return Ok(products);
        }
       
       

        
        [HttpGet("search/{productName:alpha}")]
        public async Task<ActionResult<ProductViewDTO>> GetBySearch(string productName)
        {
            var product =await _productService.Search(productName);
            return Ok(product);
        }

        [HttpGet("Pagination")]
        public async Task<ActionResult<ProductViewDTO>> GetPaginated(int page, int pageSize)
        {
            var products = await _productService.GetProductsByPagination(page, pageSize);
            
            return Ok(products);
        }
        
    }
}
