using OMSBlazor.Northwind.OrderAggregate.Exceptions;
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

            OrderDate = DateTime.Now;
        }

        public Guid EmployeeId { get; private set; }

        public Guid CustomerId { get; private set; }

        /// <summary>
        /// You can treat this as ShipperId. Don't change name of this property it should match with db column name
        /// </summary>
        public Guid ShipVia { get; private set; }

        public double Freight { get; }

        public DateTime OrderDate { get; private set; }

        public DateTime RequiredDate { get; set; }

        public ShipData? ShipData { get; private set; }

        public List<OrderDetail> OrderDetails { get; } = new();

        public void AddOrderDetail(Guid productId, int quantity, double unitPrice, float discount)
        {
            if (OrderDetails.Any(x => x.ProductId == productId))
            {
                throw new DuplicateProductException();
            }

            var orderDetail = new OrderDetail(Id, productId);
            orderDetail.Quantity = quantity;
            orderDetail.UnitPrice = unitPrice;
            orderDetail.Discount = discount;

            OrderDetails.Add(orderDetail);
        }
    }
}
