using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OMSBlazor.Northwind.OrderAggregate.Exceptions
{
    public class CustomerNameDuplicationException : BusinessException
    {
        public CustomerNameDuplicationException()
            : base(message: "Current name duplicated. Try another name")
        {

        }
    }
}
