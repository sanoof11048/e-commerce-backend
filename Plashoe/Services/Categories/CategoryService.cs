using Microsoft.EntityFrameworkCore;
using Plashoe.Data;
using Plashoe.Model;

namespace Plashoe.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<Category>>> GetCategoires()
        {
            var categories =  await _context.Category.ToListAsync();
            if (categories == null)
            {
                return new ApiResponse<List<Category>>(404, "Categories Not Found", categories, "Not Found");

            }
            return new ApiResponse<List<Category>> (200, "Categories Fetched Successfull", categories);
        }

        public async Task<ApiResponse<Category>> AddCategory(Category category)
        {
            var isExist = _context.Category.FirstOrDefaultAsync(c=>c.Name == category.Name);
            if (isExist != null)
            {
                return new ApiResponse<Category>(400,"Category is Already Exist", category);
            }
            return new ApiResponse<Category>(201, "Category Added", category);
        }

        public async Task<ApiResponse<bool>> DeleteCategory(int id)
        {
            var isExist = _context.Category.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (isExist == null)
            {
                return new ApiResponse<bool>(404, "Category is Not Exist",false,"Category is not Found");
            }
            return new ApiResponse<bool>(200,"Categroy Deleted", true);
        }
       
    }
}

