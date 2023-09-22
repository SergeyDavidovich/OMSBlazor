using OMSBlazor.Dto.Product;
using OMSBlazor.Application.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using OMSBlazor.Northwind.ProductAggregate;
using AutoMapper.Internal.Mappers;
using Volo.Abp.Application.Services;

namespace OMSBlazor.Application.ApplicationServices
{
    public class ProductApplicationService : ApplicationService, IProductApplicationService
    {
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<Category, int> _categoryRepository;

        public ProductApplicationService(IRepository<Product, int> productRepository, IRepository<Category, int> categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            var products = await _productRepository.GetListAsync();

            var productsDto = products.Select(x => ObjectMapper.Map<Product, ProductDto>(x)).ToList();

            return productsDto;
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _categoryRepository.GetListAsync();
            
            var categoriesDto = ObjectMapper.Map<List<Category>, List<CategoryDto>>(categories);

            return categoriesDto;
        }
    }
}
