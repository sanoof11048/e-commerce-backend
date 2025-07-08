using Plashoe.DTOs.Products;
using Plashoe.DTOs;
using Plashoe.Repositories;
using Plashoe.Data;
using AutoMapper;
using Plashoe.Model;
using Microsoft.EntityFrameworkCore;
using Plashoe.Services.Cloudinary;

namespace Plashoe.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly ProductRepository _repo;
        private readonly ILogger<AdminService> _logger;
        private readonly IMapper _mapper;
        private readonly OrderRepository _orderRepo;
        private readonly ICloudinaryService _cloudinaryService;

        public AdminService(ICloudinaryService cloudinaryService,ProductRepository repo,OrderRepository repository, IMapper mapper, ILogger<AdminService> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
            _orderRepo = repository;
            _cloudinaryService = cloudinaryService;
        }
        public async Task<ApiResponse<object>> EditProduct(int productId, ProductViewDTO updatedProductDto)
        {
            try
            {
                var existingProduct = await _repo.GetById(productId);
                if (existingProduct == null)
                {
                    return new ApiResponse<object>(404, "Product not found", null, "Not Found");
                }

                var category = await _repo.GetCategoryById(updatedProductDto.CategoryId);
                if (category == null)
                {
                    return new ApiResponse<object>(400, "Invalid category", null, "Category does not exist for the given CategoryId.");
                }

                existingProduct.ProductName = updatedProductDto.ProductName;
                existingProduct.Price = updatedProductDto.Price;
                existingProduct.Description = updatedProductDto.Description;
                existingProduct.CategoryId = updatedProductDto.CategoryId;
                existingProduct.Stock = updatedProductDto.Stock;

                if (updatedProductDto.ImageFile != null)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(updatedProductDto.ImageFile);
                    existingProduct.Image = imageUrl;
                }
                else
                {
                    existingProduct.Image = updatedProductDto.Image;
                }

                await _repo.Update(existingProduct);

                return new ApiResponse<object>(200, "Product updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product");
                return new ApiResponse<object>(500, "An error occurred while updating product", null, ex.Message);
            }
        }

        public async Task<ApiResponse<object>> DeleteProduct(int id)
        {
            try
            {
                var product = await _repo.GetById(id);
                if (product == null)
                {
                    return new ApiResponse<object>(404, "Product not found", null, "No product with this ID");
                }

                bool deleted = await _repo.DeleteAsync(product);

                return deleted
                    ? new ApiResponse<object>(200, "Product deleted successfully")
                    : new ApiResponse<object>(500, "Delete operation failed", null, "Unknown error occurred during deletion");
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>(500, "An error occurred while deleting product", null, ex.Message);
            }
        }

        public async Task<ApiResponse<object>> AddProduct(ProductViewDTO productViewDTO)
        {
            try
            {
                if (await _repo.GetByName(productViewDTO.ProductName) != null)
                {
                    return new ApiResponse<object>(400, "Product already exists", null, "Duplicate product name");
                }

                var category = await _repo.GetCategoryById(productViewDTO.CategoryId);
                if (category == null)
                {
                    return new ApiResponse<object>(400, "Invalid category", null, "Category does not exist for this ID");
                }

                var product = _mapper.Map<Product>(productViewDTO);
                product.CategoryId = productViewDTO.CategoryId;
                if (productViewDTO.ImageFile != null)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(productViewDTO.ImageFile);
                    Console.WriteLine(imageUrl);
                    product.Image = imageUrl;
                }


                await _repo.Add(product);

                return new ApiResponse<object>(200, "Product added successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>(500, "Error adding product", null, ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<OrderDTO>>> GetAllOrders()
        {
            var orders = await _orderRepo.GetAllOrders();

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
        public async Task<ApiResponse<string>> UpdateOrderStaus(int orderId, string status)
        {
            var order = await _orderRepo.GetOrderDetails(orderId);
            order.Status = status;
            await _orderRepo.Save();
            return new ApiResponse<string>(200, "Orders Status Updated", "Order " );
        }
    }
}
