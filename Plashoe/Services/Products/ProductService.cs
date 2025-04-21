using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plashoe.Data;
using Plashoe.DTOs;
using Plashoe.DTOs.Products;
using Plashoe.Model;
using Plashoe.Repositories;
using System.Security.Claims;

namespace Plashoe.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _repo;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;

        public ProductService(ProductRepository repository , IMapper mapper, ILogger<ProductService> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _repo = repository;

        }

        public async Task<List<ProductViewDTO>> GetAllProducts()
        {
            try
            {
                var _products = await _repo.GetAll();
                return _mapper.Map<List<ProductViewDTO>>(_products);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<ProductViewDTO> GetProductById(int id)
        {
            try
            {
                var products = await _repo.GetById(id);
                return products == null? null: _mapper.Map<ProductViewDTO>(products);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProductViewDTO>> GetProductByCategory(string category)
        {
            try
            {
                var products = await _repo.GetByCategory(category);
                return _mapper.Map<List<ProductViewDTO>>(products);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

     

        public async Task<List<ProductViewDTO>> Search(string productName)
        {
            var product = await _repo.SearchAsync(productName);
            if (product == null || product.Count == 0)
                return new List<ProductViewDTO>();

            var result = _mapper.Map<List<ProductViewDTO>>(product);
            return result;
        }

        public async Task<List<ProductViewDTO>> GetProductsByPagination(int page, int pageSize)
        {
            try
            {
                var products = await _repo.GetPaginatedAsync(page, pageSize);
                return _mapper.Map<List<ProductViewDTO>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllProducts");
                throw;
            }


        }
    }
}
