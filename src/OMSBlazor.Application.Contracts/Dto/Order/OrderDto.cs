using System;
using System.Collections.Generic;
using System.Text;

namespace OMSBlazor.Dto.Order
{
    public class OrderDto
    {
        public int OrderId { get; set; }

        public int EmployeeId { get; set; }

        public string? CustomerId { get; set; }

        /// <summary>
        /// You can treat this as ShipperId. Don't change name of this property it should match with db column name
        /// </summary>
        public int ShipVia { get; set; }

        public double Freight { get; }

        public DateTime OrderDate { get; set; }

        public DateTime RequiredDate { get; set; }

        public string ShipName { get; internal set; }

        public string ShipAddress { get; internal set; }

        public string ShipRegion { get; internal set; }

        public string ShipCity { get; internal set; }

        public string ShipPostalCode { get; internal set; }

        public string ShipCountry { get; internal set; }

        public List<OrderDetailDto> OrderDetails { get; set; }
    }
}
