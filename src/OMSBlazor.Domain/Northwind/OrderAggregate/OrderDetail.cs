using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Values;
using Volo.Abp.MultiTenancy;

namespace OMSBlazor.Northwind.OrderAggregate
{
    public class OrderDetail : Entity, IMultiTenant
    {
        public OrderDetail(
            int orderId, 
            int productId)
        {
            OrderId = orderId;
            ProductId = productId;
        }

        public int OrderId { get; private set; }

        public int ProductId { get; private set; }

        public double UnitPrice { get; internal set; }

        public int Quantity { get; internal set; }

        public float Discount { get; internal set; }

        public Guid? TenantId { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { OrderId, ProductId };
        }
    }
}
