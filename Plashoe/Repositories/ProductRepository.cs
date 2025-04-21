using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Plashoe.Data;
using Plashoe.DTOs.Products;
using Plashoe.Model;
using Plashoe.DTOs;

namespace Plashoe.Repositories
{
    public class ProductRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(AppDbContext context, IMapper mapper, ILogger<ProductRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<Product>> GetAll() =>
            await _context.Products.Include(p => p.Category).ToListAsync();

        public async Task<Product> GetById(int id) =>
            await _context.Products.FindAsync(id);

        public async Task<List<Product>> GetByCategory(string category) =>
            await _context.Products.Include(p => p.Category)
                                   .Where(p => p.Category.Name == category)
                                   .ToListAsync();

        public async Task<Product?> GetByName(string name) =>
            await _context.Products.FirstOrDefaultAsync(p => p.ProductName == name);

        public async Task Add(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Saving product: {product.ProductName}, Price: {product.Price}, CategoryId: {product.CategoryId}");

        }

        public async Task<bool> DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Product>> SearchAsync(string keyword) =>
            await _context.Products.Include(p => p.Category)
                                   .Where(p => p.ProductName.ToLower().Contains(keyword.ToLower()))
                                   .ToListAsync();

        public async Task<List<Product>> GetPaginatedAsync(int page, int size) =>
            await _context.Products.Include(p => p.Category)
                                   .Skip((page - 1) * size)
                                   .Take(size)
                                   .ToListAsync();

        public async Task<Category> GetCategoryById(int Id)=>
            await _context.Category.FirstOrDefaultAsync(x => x.CategoryId == Id);

        public async Task Update(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

    }
}
