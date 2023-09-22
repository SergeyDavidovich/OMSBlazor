﻿using OMSBlazor.Dto.Product;
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
    }
}
