using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Values;

namespace OMSBlazor.Northwind.OrderAggregate
{
    public class OrderDetail : Entity
    {
        public OrderDetail(
            Guid orderId, 
            Guid productId)
        {
            OrderId = orderId;
            ProductId = productId;
        }

        public Guid OrderId { get; private set; }

        public Guid ProductId { get; private set; }

        public double UnitPrice { get; internal set; }

        public int Quantity { get; internal set; }

        public float Discount { get; internal set; }

        public override object[] GetKeys()
        {
            return new object[] { OrderId, ProductId };
        }
    }
}
