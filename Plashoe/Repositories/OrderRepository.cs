using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Plashoe.Data;
using Plashoe.DTOs.Products;
using Plashoe.Model;

namespace Plashoe.Repositories
{
    public class OrderRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductRepository> _logger;

        public OrderRepository(AppDbContext context, IMapper mapper, ILogger<ProductRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<Order>> GetOrdersById(int userId)=>
            await _context.Order
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync();

        public async Task<Cart> GetCartById(int userId)=>
        await _context.Carts
                    .FirstOrDefaultAsync(c => c.UserId == userId);

        public async Task EmpyCart(int cartId)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found");
            }
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        public async Task AddOrder(Order order)
        {
            await _context.Order.AddAsync(order);
            await Save();
        }

        public async Task RemoveOrder(Order order)
        {
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrderDetails(int orderId)=>
            await _context.Order.FindAsync(orderId);

        public async Task<List<Order>> GetAllOrders()=>
            await _context.Order
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync();

        public async Task Save()=>
            await _context.SaveChangesAsync();
    }
}
