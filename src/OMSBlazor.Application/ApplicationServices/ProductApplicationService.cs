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

        public ProductApplicationService(IRepository<Product, int> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            var products = await _productRepository.GetQueryableAsync();

            var productsDto = products.Select(x => ObjectMapper.Map<Product, ProductDto>(x)).ToList();

            return productsDto;
        }
    }
}
