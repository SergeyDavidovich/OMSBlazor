using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OMSBlazor.Northwind.OrderAggregate.Exceptions
{
    public class EmptyCategoryNameException : BusinessException
    {
        public EmptyCategoryNameException()
            : base(message: "Category name cannot be empty")
        {

        }
    }
}
