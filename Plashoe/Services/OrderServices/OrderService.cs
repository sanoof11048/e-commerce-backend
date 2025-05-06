using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Plashoe.Data;
using Plashoe.DTOs.Products;
using Plashoe.Model;
using Plashoe.Repositories;

namespace Plashoe.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly OrderRepository _repo;

        public OrderService(ILogger<OrderService> logger, OrderRepository repository)
        {
            _logger = logger;
            _repo = repository;
        }

        public async Task<ApiResponse<List<OrderDTO>>> GetOrders(int userId)
        {
            try
            {
                var orders = await _repo.GetOrdersById(userId);

                // Map orders to DTOs
                var orderDTOs = orders.Select(o => new OrderDTO
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    TransactionId = o.TransactionId,
                    Status = o.Status,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemDTO
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.ProductName,
                        Quantity = oi.Quantity,
                        TotalPrice = oi.TotalPrice,
                    }).ToList()
                }).ToList();

                return new ApiResponse<List<OrderDTO>>(200, "Orders fetched successfully", orderDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching orders");
                return new ApiResponse<List<OrderDTO>>(500, "Error fetching orders", null, ex.Message);
            }
        }


        public async Task<ApiResponse<string>> PlaceOrder(OrderCreateDTO orderDto)
        {
            try
            {
                
                var order = new Order
                {
                    UserId = orderDto.UserId,
                    AddressId = orderDto.AddressId,
                    OrderDate = DateTime.UtcNow,
                    TransactionId = orderDto.TransactionId,
                    Status = "placed",
                    OrderItems = orderDto.OrderItems.Select(i => new OrderItem
                    {
                        ProductId = i.ProductId,
                        ProductName = i.ProductName,
                        Quantity = i.Quantity,
                        TotalPrice = i.TotalPrice
                    }).ToList(),
                    TotalAmount = orderDto.OrderItems.Sum(i => i.TotalPrice)
                };

                await _repo.AddOrder(order);

                var cart = await _repo.GetCartById(orderDto.UserId);

                if (cart != null)
                {
                   await _repo.EmpyCart(cart.Id);
                }


                return new ApiResponse<string>(200, "Order placed successfully", order.Id.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error placing order");
                return new ApiResponse<string>(500, "Error placing order", null, ex.Message);
            }
        }




        public async Task<ApiResponse<string>> CancelOrder(int orderId)
        {
            try
            {
                var order = await _repo.GetOrderDetails(orderId);
                if (order == null)
                    return new ApiResponse<string>(404, "Order not found");

                await _repo.RemoveOrder(order);

                return new ApiResponse<string>(200, "Order removed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing order");
                return new ApiResponse<string>(500, "Error removing order", null, ex.Message);
            }
        }

    }

}
