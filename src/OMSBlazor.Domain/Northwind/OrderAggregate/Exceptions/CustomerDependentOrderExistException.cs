using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OMSBlazor.Northwind.OrderAggregate.Exceptions
{
    public class CustomerDependentOrderExistException : BusinessException
    {
        public CustomerDependentOrderExistException(int orderId) 
            : base(message: $"Current customer have dependency with order with id {orderId}")
        { }
    }
}
