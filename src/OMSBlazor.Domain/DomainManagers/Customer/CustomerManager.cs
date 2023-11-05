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

namespace OMSBlazor.DomainManagers.Customer
{
    public class CustomerManager : DomainService, ICustomerManager
    {
        private readonly IRepository<Northwind.OrderAggregate.Customer, string> _customerRepository;
        private readonly IRepository<Northwind.OrderAggregate.Order, int> _orderRepository;

        public CustomerManager(
            IRepository<Northwind.OrderAggregate.Customer, string> customerRepository, 
            IRepository<Northwind.OrderAggregate.Order, int> orderRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }

        public async Task<Northwind.OrderAggregate.Customer> CreateAsync(string name)
        {
            var customers = await _customerRepository.GetListAsync();
            if (customers.Any(x => x.CompanyName == name))
            {
                throw new CustomerNameDuplicationException();
            }

            var key = customers.Last().Id + 1;

            var customer = new Northwind.OrderAggregate.Customer(key, name);

            return customer;
        }

        public async Task ThrowIfCannotDeleteAsync(string id)
        {
            if (!(await _customerRepository.AnyAsync(x => x.Id == id)))
            {
                throw new EntityNotFoundException(typeof(Northwind.OrderAggregate.Category), id);
            }

            var dependentOrder = await _orderRepository.FirstOrDefaultAsync(x => x.CustomerId == id);
            if (dependentOrder is not null)
            {
                throw new CustomerDependentOrderExistException(dependentOrder.Id);
            }
        }
    }
}
