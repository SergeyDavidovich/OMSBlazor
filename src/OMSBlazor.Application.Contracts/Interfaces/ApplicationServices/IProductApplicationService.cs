﻿using OMSBlazor.Dto.Category;
using OMSBlazor.Dto.Product;
using OMSBlazor.Dto.Product.Stastics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OMSBlazor.Interfaces.ApplicationServices
{
    public interface IProductApplicationService : IApplicationService
    {
        public Task<List<ProductDto>> GetProductsAsync();

        public Task<ProductDto> GetProductAsync(int id);

        public Task DeleteProductAsync(int id);

        public Task CreateProductAsync(CreateProductDto productDto);

        public Task UpdateProductAsync(int id, UpdateProductDto productDto);

        public Task<IEnumerable<ProductsByCategoryDto>> GetProductsByCategoryAsync();
    }
}
