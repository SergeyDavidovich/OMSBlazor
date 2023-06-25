using OMSBlazor.Northwind.CustomerAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace OMSBlazor.DomainManagers.Customer
{
    public class CustomerManager : ICustomerManager
    {
        private readonly IRepository<Northwind.CustomerAggregate.Customer, int> customerRepository;

        public CustomerManager(IRepository<Northwind.CustomerAggregate.Customer, int> customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public async Task<Northwind.CustomerAggregate.Customer> CreateAsync(string name)
        {
            var customers = await customerRepository.GetListAsync();
            if (customers.Any(x => x.CompanyName == name))
            {
                throw new CustomerNameDuplicationException();
            }

            var key = customers.Last().Id + 1;

            var customer = new Northwind.CustomerAggregate.Customer(key, name);

            await customerRepository.InsertAsync(customer);

            return customer;
        }
    }
}
