using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OMSBlazor.Northwind.OrderAggregate.Exceptions
{
    public class EmptyLastNameException : BusinessException
    {
        public EmptyLastNameException()
            : base(message: "Employee's last name cannot be empty")
        {

        }
    }
}
