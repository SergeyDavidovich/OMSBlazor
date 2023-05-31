using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Northwind.ProductAggregate
{
    public class Product : AggregateRoot<Guid>
    {
        public Product(Guid id) :
            base(id)
        {

        }
    }
}
