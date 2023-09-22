using OMSBlazor.Northwind.CustomerAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace OMSBlazor.DomainManagers.Customer
{
    public class CustomerManager : DomainService, ICustomerManager
    {
        private readonly IRepository<Northwind.CustomerAggregate.Customer, string> customerRepository;

        public CustomerManager(IRepository<Northwind.CustomerAggregate.Customer, string> customerRepository)
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
