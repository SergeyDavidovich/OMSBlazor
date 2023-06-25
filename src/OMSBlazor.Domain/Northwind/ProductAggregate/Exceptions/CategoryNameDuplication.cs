using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OMSBlazor.Northwind.ProductAggregate.Exceptions
{
    public class CategoryNameDuplication : BusinessException
    {
        public CategoryNameDuplication()
            :base(message:"Current name already exist. Try another one")
        {
        }
    }
}
