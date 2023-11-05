using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OMSBlazor.Northwind.OrderAggregate.Exceptions
{
    public class DependentOrderDetailExistException : BusinessException
    {
        public DependentOrderDetailExistException(int productId, int orderId)
            : base(message: $"Product with id - {productId}, exist in order with id - {orderId}")
        {
            
        }
    }
}
