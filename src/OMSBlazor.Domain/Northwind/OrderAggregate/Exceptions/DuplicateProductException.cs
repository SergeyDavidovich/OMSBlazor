using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OMSBlazor.Northwind.OrderAggregate.Exceptions
{
    public class DuplicateProductException : BusinessException
    {
        // TODO: Taking this message from localization resource
        public DuplicateProductException() 
            : base(message: "This product already exist in order")
        { }
    }
}
