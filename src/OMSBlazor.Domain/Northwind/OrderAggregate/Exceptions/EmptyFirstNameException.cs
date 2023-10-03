using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OMSBlazor.Northwind.OrderAggregate.Exceptions
{
    public class EmptyFirstNameException : BusinessException
    {
        public EmptyFirstNameException()
            : base(message: "Employee's first name cannot be empty")
        {

        }
    }
}
