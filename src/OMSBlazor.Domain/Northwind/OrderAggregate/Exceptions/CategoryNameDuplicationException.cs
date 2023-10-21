using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OMSBlazor.Northwind.OrderAggregate.Exceptions
{
    public class CategoryNameDuplicationException : BusinessException
    {
        public CategoryNameDuplicationException()
            : base(message: "Current name already exist. Try another one")
        {
        }
    }
}
