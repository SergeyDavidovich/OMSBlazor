using OMSBlazor.Dto.Product;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.Interfaces
{
    public interface IProductApplicationService
    {
        public Task<List<ProductDto>> GetProductsAsync();
    }
}
