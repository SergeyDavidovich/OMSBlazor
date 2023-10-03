using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OMSBlazor.Northwind.OrderAggregate.Exceptions
{
    public class EmptyCustomerNameException : BusinessException
    {
        public EmptyCustomerNameException()
            : base(message: "Customer name cannot be empty")
        {
        }
    }
}
