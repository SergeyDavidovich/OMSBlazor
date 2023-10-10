using OMSBlazor.Northwind.OrderAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Northwind.OrderAggregate
{
    public class Order : AggregateRoot<int>
    {
        private Order() { }

        public Order(
            int id, 
            int employeeId, 
            string customerId) :
            base(id)
        {
            EmployeeId = employeeId;
            CustomerId = customerId;

            OrderDate = DateTime.Now;
        }

        public int EmployeeId { get; private set; }

        public string? CustomerId { get; private set; }

        public double Freight { get; }

        public DateTime OrderDate { get; private set; }

        public DateTime RequiredDate { get; set; }

        public string? ShipName { get; set; }

        public string? ShipAddress { get; set; }

        public string? ShipRegion { get; set; }

        public string? ShipCity { get; set; }

        public string? ShipPostalCode { get; set; }

        public string? ShipCountry { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; private set; }

        public void AddOrderDetail(int productId, int quantity, double unitPrice, float discount)
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
