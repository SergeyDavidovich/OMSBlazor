using OMSBlazor.Dto.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using AutoMapper.Internal.Mappers;
using Volo.Abp.Application.Services;
using OMSBlazor.Northwind.OrderAggregate;
using OMSBlazor.Dto.Category;
using OMSBlazor.DomainManagers.Product;
using Microsoft.AspNetCore.SignalR.Client;
using OMSBlazor.NotificationSender.Signalr;
using OMSBlazor.Dto.Product.Stastics;
using OMSBlazor.Northwind.Stastics;
using OMSBlazor.Interfaces.ApplicationServices;

namespace OMSBlazor.Application.ApplicationServices
{
    public class ProductApplicationService : ApplicationService, IProductApplicationService
    {
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<ProductsByCategory, int> _productsByCategoryRepository;
        private readonly IProductManager _productManager;

        public ProductApplicationService(
            IRepository<Product, int> productRepository,
            IProductManager productManager,
            IRepository<ProductsByCategory, int> productsByCategoryRepository)
        {
            _productRepository = productRepository;
            _productManager = productManager;
            _productsByCategoryRepository = productsByCategoryRepository;
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            var products = await _productRepository.GetListAsync();

            var productsDto = ObjectMapper.Map<List<Product>, List<ProductDto>>(products);

            return productsDto;
        }

        public async Task<ProductDto> GetProductAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);

            var productDto = ObjectMapper.Map<Product, ProductDto>(product);

            return productDto;
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productManager.ThrowIfCannotDeleteAsync(id);

            await _productRepository.DeleteAsync(id);
        }

        public async Task CreateProductAsync(CreateProductDto productDto)
        {
            var product = await _productManager.CreateAsync(productDto.ProductName, productDto.CategoryId);
            product.SetProductName(productDto.ProductName);
            product.Discontinued = productDto.Discontinued;
            product.UnitsOnOrder = productDto.UnitsOnOrder;
            product.QuantityPerUnit = productDto.QuantityPerUnit;
            product.UnitPrice = productDto.UnitPrice;
            product.ReorderLevel = productDto.ReorderLevel;
            product.UnitsInStock = productDto.UnitsInStock;

            await _productRepository.InsertAsync(product);
        }

        public async Task UpdateProductAsync(int id, UpdateProductDto productDto)
        {
            var product = await _productManager.UpdateAsync(id, productDto.ProductName);
            product.Discontinued = productDto.Discontinued;
            product.UnitsOnOrder = productDto.UnitsOnOrder;
            product.QuantityPerUnit = productDto.QuantityPerUnit;
            product.UnitPrice = productDto.UnitPrice;
            product.ReorderLevel = productDto.ReorderLevel;
            product.UnitsInStock = productDto.UnitsInStock;

            await _productRepository.UpdateAsync(product);
        }

        public async Task<IEnumerable<ProductsByCategoryDto>> GetProductsByCategoryAsync()
        {
            var stastics = await _productsByCategoryRepository.GetListAsync();

            var stasticsDto = ObjectMapper.Map<List<ProductsByCategory>, List<ProductsByCategoryDto>>(stastics);

            return stasticsDto;
        }
    }
}
