using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OMSBlazor.Dto.Order
{
    public class CreateOrderDto
    {
        [Required]
        public string CustomerId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public List<OrderDetailDto> OrderDetails { get; set; }

        public DateTime RequiredDate { get; set; }

        public string? ShipName { get; set; }

        public string? ShipAddress { get; set; }

        public string? ShipRegion { get; set; }

        public string? ShipCity { get; set; }

        public string? ShipPostalCode { get; set; }

        public string? ShipCountry { get; set; }

        public double Freight { get; }
    }
}
