using Plashoe.DTOs;
using Plashoe.DTOs.Products;
using Plashoe.Model;

namespace Plashoe.Services.Products
{
    public interface IProductService
    {
       Task<List<ProductViewDTO>> GetAllProducts();
        Task<ProductViewDTO> GetProductById(int id);
        Task<List<ProductViewDTO>> GetProductByCategory(string category);
       
        Task<List<ProductViewDTO>> Search(string productName);
        Task <List<ProductViewDTO>> GetProductsByPagination(int page, int pageSize);
       

    }
}
