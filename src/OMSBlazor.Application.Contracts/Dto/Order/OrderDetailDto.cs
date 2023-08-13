using System;
using System.Collections.Generic;
using System.Text;

namespace OMSBlazor.Dto.Order
{
    public class OrderDetailDto
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public double UnitPrice { get; set; }

        public float Discount { get; set; }
    }
}
