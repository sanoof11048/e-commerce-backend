using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Plashoe.Data;
using Plashoe.DTOs.Products;
using Plashoe.Model;

namespace Plashoe.Services.Carts
{
    public class CartsService : ICartsService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CartsService> _logger;
        private readonly IMapper _mapper;

        public CartsService(AppDbContext context, ILogger<CartsService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> AddToCart(int userId, int productId)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.Cartitems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserId = userId,
                        Cartitems = new List<CartItems>()
                    };

                    _context.Carts.Add(cart);
                }

                var existingItem = cart.Cartitems?.FirstOrDefault(ci => ci.ProductId == productId);

                if (existingItem != null)
                {
                    existingItem.Quantity += 1;
                    await _context.SaveChangesAsync();

                    return new ApiResponse<string>(200, "Product Quantity Increased");
                }
                else
                {
                    var newItem = new CartItems
                    {
                        ProductId = productId,
                        Quantity = 1
                    };

                    cart.Cartitems ??= new List<CartItems>();
                    cart.Cartitems.Add(newItem);
                    await _context.SaveChangesAsync();

                    return new ApiResponse<string>(200, "Added to cart");
                }

                
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, "Failed to add product", null, ex.Message);
            }
        }

        public async Task<ApiResponse<CartViewDTO>> GetCartDetails(int userId)
        {
            try
            {
                if (userId == 0)
                {
                    return new ApiResponse<CartViewDTO>(400, "Invalid User ID", null, "Userid is null");
                }

                var cart = await _context.Carts
                    .Include(c => c.Cartitems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null || cart.Cartitems == null || !cart.Cartitems.Any())
                    return new ApiResponse<CartViewDTO>(404, "Cart is empty", null, "No items found");

                var dto = _mapper.Map<CartViewDTO>(cart);
                dto.TotalPrice = dto.Items.Sum(i => i.Price.GetValueOrDefault() * i.Quantity);

                return new ApiResponse<CartViewDTO>(200, "Cart fetched successfully", dto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CartViewDTO>(500, "Failed to fetch cart", null, ex.Message);
            }
        }

        public async Task<ApiResponse<string>> RemoveCartItem(int userId, int productId)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.Cartitems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null || cart.Cartitems == null)
                    return new ApiResponse<string>(404, "Cart not found", null, "No cart items found");

                var item = cart.Cartitems.FirstOrDefault(ci => ci.ProductId == productId);

                if (item == null)
                    return new ApiResponse<string>(404, "Item not found", null, "Product doesn't exist in cart");

                cart.Cartitems.Remove(item);
                await _context.SaveChangesAsync();
                return new ApiResponse<string>(200, "Product removed from cart");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, "Error removing item", null, ex.Message);
            }
        }

        public async Task<ApiResponse<string>> IncreaseQuantity(int userId, int productId)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.Cartitems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null || cart.Cartitems == null)
                    return new ApiResponse<string>(404, "Cart not found", null, "No cart items found");

                var item = cart.Cartitems.FirstOrDefault(ci => ci.ProductId == productId);
                if (item == null)
                    return new ApiResponse<string>(404, "Item not found", null, "Product doesn't exist in cart");

                item.Quantity += 1;
                await _context.SaveChangesAsync();
                return new ApiResponse<string>(200, "Quantity increased");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, "Error increasing quantity", null, ex.Message);
            }
        }

        public async Task<ApiResponse<string>> DecreaseQuantity(int userId, int productId)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.Cartitems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null || cart.Cartitems == null)
                    return new ApiResponse<string>(404, "Cart not found", null, "No cart items found");

                var item = cart.Cartitems.FirstOrDefault(ci => ci.ProductId == productId);
                if (item == null)
                    return new ApiResponse<string>(404, "Item not found", null, "Product doesn't exist in cart");

                if (item.Quantity > 1)
                {

                    item.Quantity -= 1;
                }
                else
                {
                    return new ApiResponse<string>(400, "Quantity Can't Decrease");
                }

                await _context.SaveChangesAsync();
          

                return new ApiResponse<string>(200, "Quantity decreased");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, "Error decreasing quantity", null, ex.Message);
            }
        }
    }
}
