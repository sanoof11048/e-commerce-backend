using Plashoe.Model;

namespace Plashoe.Services.Categories
{
    public interface ICategoryService
    {
        Task<ApiResponse<List<Category>>> GetCategoires();
        Task<ApiResponse<Category>> AddCategory(Category category);
        Task<ApiResponse<bool>> DeleteCategory(int id);

    }
}
