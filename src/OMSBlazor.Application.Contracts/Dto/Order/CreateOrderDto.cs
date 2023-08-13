using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OMSBlazor.Dto.Order
{
    public class CreateOrderDto
    {
        [Required]
        public int CustomerId { get; }

        [Required]
        public int EmployeeId { get; }

        [Required]
        public List<OrderDetailDto> OrderDetails { get; }
    }
}
