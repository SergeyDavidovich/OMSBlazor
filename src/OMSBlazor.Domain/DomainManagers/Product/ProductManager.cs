using OMSBlazor.Northwind.OrderAggregate;
using OMSBlazor.Northwind.OrderAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace OMSBlazor.DomainManagers.Product
{
    public class ProductManager : DomainService, IProductManager
    {
        private readonly IRepository<Northwind.OrderAggregate.Product, int> _productRepository;
        private readonly IRepository<Northwind.OrderAggregate.OrderDetail> _orderDetailRepository;

        public ProductManager(IRepository<Northwind.OrderAggregate.Product, int> productRepository, IRepository<OrderDetail> orderDetailRepository)
        {
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<Northwind.OrderAggregate.Product> CreateAsync(string name, int categoryId)
        {
            var products = await _productRepository.GetListAsync();
            if (products.Any(x => x.ProductName == name))
            {
                throw new ProductNameDuplicationException();
            }

            var key = products.Last().Id + 1;

            var product = new Northwind.OrderAggregate.Product(key, name, categoryId);

            await _productRepository.InsertAsync(product);

            return product;
        }

        public async Task ThrowIfCannotDeleteAsync(int id)
        {
            var dependentOrderDetail = await _orderDetailRepository.FirstOrDefaultAsync(x => x.ProductId == id);

            if (dependentOrderDetail is not null)
            {
                throw new DependentOrderDetailExistException(id, dependentOrderDetail.OrderId);
            }
        }
    }
}
