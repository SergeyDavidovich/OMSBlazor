﻿using OMSBlazor.Northwind.ProductAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace OMSBlazor.DomainManagers.Product
{
    public class ProductManager : IProductManager
    {
        private readonly IRepository<Northwind.ProductAggregate.Product, int> productRepository;

        public ProductManager(IRepository<Northwind.ProductAggregate.Product, int> productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<Northwind.ProductAggregate.Product> CreateAsync(string name, Northwind.ProductAggregate.Category category)
        {
            var products = await productRepository.GetListAsync();
            if (products.Any(x => x.ProductName == name))
            {
                throw new ProductNameDuplicationException();
            }

            var key = products.Last().Id + 1;

            var product = new Northwind.ProductAggregate.Product(key, name, category);

            await productRepository.InsertAsync(product);

            return product;
        }
    }
}