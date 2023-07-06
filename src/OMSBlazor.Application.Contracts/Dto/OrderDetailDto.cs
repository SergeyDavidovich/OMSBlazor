using System;
using System.Collections.Generic;
using System.Text;

namespace OMSBlazor.Dto
{
    public class OrderDetailDto
    {
        public int ProductId { get; set; }

        public double UnitPrice { get; set; }

        public float Discount { get; set; }
    }
}
