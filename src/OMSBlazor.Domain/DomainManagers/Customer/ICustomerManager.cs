using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.DomainManagers.Customer
{
    public interface ICustomerManager 
    {
        public Task<Northwind.OrderAggregate.Customer> CreateAsync(string name);

        public Task DeleteAsync(string id);
    }
}
