using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Northwind.OrderAggregate
{
    public class Order : AggregateRoot<Guid>
    {
        private Order() { }

        public Order(Guid id) :
            base(id)
        {

        }
    }
}
