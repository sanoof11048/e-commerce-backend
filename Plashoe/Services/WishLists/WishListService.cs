using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Plashoe.Data;
using Plashoe.DTOs;
using Plashoe.DTOs.Products;
using Plashoe.Model;
using Plashoe.Repositories;
using Plashoe.Services.Products;
using Plashoe.Services.WishLists;

namespace Plashoe.Services.WishList
{
    public class WishListService : IWishListService
    {
        private readonly AppDbContext _context;
        private readonly ProductRepository _repo;
        private readonly ILogger<WishListService> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public WishListService(ProductRepository productRepository, AppDbContext context, IMapper mapper, ILogger<WishListService> logger, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _repo = productRepository;
        }

        public async Task<ApiResponse<object>> AddtoWish(int userId, int productId)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {userId} does not exist.");
                    return new ApiResponse<object>(400, "User does not exist.", null, $"User with ID {userId} does not exist.");
                }

                var productExist = await _repo.GetById(productId);
                if (productExist == null)
                {
                    _logger.LogWarning($"Product with ID {productId} does not exist.");
                    return new ApiResponse<object>(400, "Product does not exist.", null, $"Product with ID {productId} does not exist.");
                }

                var isexist = await _context.Wishlist.FirstOrDefaultAsync(p => p.ProductId == productId && p.UserId == userId);
                if (isexist == null)
                {
                    WishlistDTO wishListDto = new WishlistDTO()
                    {
                        ProductId = productId,
                        UserId = userId,
                    };
                    var wish = _mapper.Map<Wishlist>(wishListDto);
                    _context.Wishlist.Add(wish);
                    await _context.SaveChangesAsync();

                    return new ApiResponse<object>(200, "Product Added To WishList Successfully", null);
                }
                else
                {

                return new ApiResponse<object>(200, "Product already exists in wishlist", null, "Product already in wishlist");
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>(500, "An error occurred.", null, ex.Message);
            }
        }

        public async Task<List<Wishlist>> GetWishList(int userId)
        {
            var wishList = await _context.Wishlist
                .Where(w => w.UserId == userId)
                .Include(w => w.Products)
                .ToListAsync();

            return wishList;
        }

        public async Task<ApiResponse<object>> DeleteWishList(int userId, int productId)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
                var wishList = await _context.Wishlist
                    .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

                if (wishList == null)
                {
                    return new ApiResponse<object>(400, "Product not found in wishlist.", null, "Bad Request");
                }

                _context.Wishlist.Remove(wishList);
                await _context.SaveChangesAsync();

                return new ApiResponse<object>(200, "Product Removed From Wishlist");
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>(500, "An error occurred.", null, ex.Message);
            }
        }
    }
}
