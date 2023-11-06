﻿using OMSBlazor.Dto.Product;
using OMSBlazor.Application.Contracts.Interfaces;
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

namespace OMSBlazor.Application.ApplicationServices
{
    public class ProductApplicationService : ApplicationService, IProductApplicationService
    {
        private readonly IRepository<Product, int> _productRepository;
        private readonly IProductManager _productManager;

        public ProductApplicationService(
            IRepository<Product, int> productRepository,
            IProductManager productManager)
        {
            _productRepository = productRepository;
            _productManager = productManager;
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
    }
}
