using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OMSBlazor.Northwind.OrderAggregate.Exceptions
{
    public class DependentProductExistException : BusinessException
    {
        public DependentProductExistException(int categoryId, int productId)
            : base(message: $"Category with id - {categoryId} have child product with id - {productId}")
        {
            
        }
    }
}
