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

        public Order(
            Guid id, 
            Guid employeeId, 
            Guid customerId) :
            base(id)
        {
            EmployeeId = employeeId;
            CustomerId = customerId;
        }

        public Guid EmployeeId { get; private set; }

        public Guid CustomerId { get; private set; }

        /// <summary>
        /// You can treat this as ShipperId. Don't change name of this property it should match with db column name
        /// </summary>
        public Guid ShipVia { get; private set; }

        public DateTime OrderDate { get; private set; }

        public DateTime RequiredDate { get; private set; }

        public ShipData ShipData { get; private set; }
    }
}
