using OMSBlazor.Northwind.OrderAggregate;
using StripeModule.Backend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.DomainManagers.Order
{
    public interface IOrderManager
    {
        public Task<Northwind.OrderAggregate.Order> CreateAsync(int employeeId, string customerId);

        public Task<Payment> PayAsync(int orderId);
    }
}
