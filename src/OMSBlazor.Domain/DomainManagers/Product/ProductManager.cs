using OMSBlazor.Northwind.OrderAggregate;
using OMSBlazor.Northwind.OrderAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace OMSBlazor.DomainManagers.Product
{
    public class ProductManager : DomainService, IProductManager
    {
        private readonly IRepository<Northwind.OrderAggregate.Product, int> _productRepository;
        private readonly IRepository<Northwind.OrderAggregate.Category, int> _categoryRepository;
        private readonly IRepository<Northwind.OrderAggregate.OrderDetail> _orderDetailRepository;

        public ProductManager(
            IRepository<Northwind.OrderAggregate.Product, int> productRepository,
            IRepository<OrderDetail> orderDetailRepository,
            IRepository<Northwind.OrderAggregate.Category, int> categoryRepository)
        {
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Northwind.OrderAggregate.Product> CreateAsync(string name, int categoryId)
        {
            var products = await _productRepository.GetListAsync();
            if (products.Any(x => x.ProductName == name))
            {
                throw new ProductNameDuplicationException();
            }

            if (!(await _categoryRepository.AnyAsync(x => x.Id == categoryId)))
            {
                throw new EntityNotFoundException(typeof(Northwind.OrderAggregate.Category), categoryId);
            }

            var key = products.Last().Id + 1;

            var product = new Northwind.OrderAggregate.Product(key, name, categoryId);

            return product;
        }

        public async Task ThrowIfCannotDeleteAsync(int id)
        {
            if (!(await _productRepository.AnyAsync(x => x.Id == id)))
            {
                throw new EntityNotFoundException(typeof(Northwind.OrderAggregate.Product), id);
            }

            var dependentOrderDetail = await _orderDetailRepository.FirstOrDefaultAsync(x => x.ProductId == id);

            if (dependentOrderDetail is not null)
            {
                throw new DependentOrderDetailExistException(id, dependentOrderDetail.OrderId);
            }
        }

        public async Task<Northwind.OrderAggregate.Product> UpdateAsync(int id, string name)
        {
            if (!(await _productRepository.AnyAsync(x => x.Id == id)))
            {
                throw new EntityNotFoundException(typeof(Northwind.OrderAggregate.Product), id);
            }

            if (await _productRepository.AnyAsync(x => x.ProductName == name && x.Id != id))
            {
                throw new ProductNameDuplicationException();
            }

            var product = await _productRepository.SingleAsync(x => x.Id == id);
            product.SetProductName(name);

            return product;
        }
    }
}
