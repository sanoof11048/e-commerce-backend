using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plashoe.Model;
using Plashoe.Services.Categories;

namespace Plashoe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var categories = await _categoryService.GetCategoires();
            return Ok(categories);
        }

        [HttpDelete("Delete/${id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteCategory(id);
            return Ok(result);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(Category category)
        {
            var result = await _categoryService.AddCategory(category);
            return Ok(result);
        }
    }
}
