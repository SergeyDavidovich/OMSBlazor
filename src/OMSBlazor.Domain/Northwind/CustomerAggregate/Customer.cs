using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Northwind.CustomerAggregate
{
    public class Customer: AggregateRoot<Guid>
    {
        public Customer(Guid id):
            base(id)
        {
            
        }
    }
}
