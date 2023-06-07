using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OMSBlazor.Northwind.ProductAggregate.Exceptions
{
    public class EmptyProductNameException : BusinessException
    {
        public EmptyProductNameException()
            :base(message: "Product name cannot be empty")
        {
            
        }
    }
}
