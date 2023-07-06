using System;
using System.Collections.Generic;
using System.Text;

namespace OMSBlazor.Dto.Order
{
    public class OrderDto
    {
        public int EmployeeId { get; set; }

        public int CustomerId { get; set; }

        /// <summary>
        /// You can treat this as ShipperId. Don't change name of this property it should match with db column name
        /// </summary>
        public int ShipVia { get; set; }

        public double Freight { get; }

        public DateTime OrderDate { get; set; }

        public DateTime RequiredDate { get; set; }

        public ShipDataDto? ShipData { get; set; }

        public List<OrderDetailDto> OrderDetails { get; set; }
    }
}
