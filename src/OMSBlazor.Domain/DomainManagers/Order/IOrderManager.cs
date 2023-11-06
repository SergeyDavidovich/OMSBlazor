using OMSBlazor.Northwind.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.DomainManagers.Order
{
    public interface IOrderManager
    {
        public Task<Northwind.OrderAggregate.Order> CreateAsync(int employeeId, string customerId, List<OrderDetail> orderDetails);
    }
}
