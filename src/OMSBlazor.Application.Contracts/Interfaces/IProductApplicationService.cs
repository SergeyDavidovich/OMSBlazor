using OMSBlazor.Dto.Category;
using OMSBlazor.Dto.Product;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OMSBlazor.Application.Contracts.Interfaces
{
    public interface IProductApplicationService : IApplicationService
    {
        public Task<List<ProductDto>> GetProductsAsync();

        public Task<ProductDto> GetProductAsync(int id);

        public Task DeleteProductAsync(int id);

        public Task CreateProductAsync(CreateProductDto productDto);

        public Task UpdateProductAsync(int id, UpdateProductDto productDto);

        public Task UpdateCurrentProductUnitsInStockAsync(int id, int currentUnitsInStock);
    }
}
