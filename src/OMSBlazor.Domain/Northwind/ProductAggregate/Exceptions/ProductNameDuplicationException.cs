using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OMSBlazor.Northwind.ProductAggregate.Exceptions
{
    public class ProductNameDuplicationException : BusinessException
    {
        public ProductNameDuplicationException()
            : base("Current name duplicated. Try another please")
        {
            
        }
    }
}
