using System;
using System.Collections.Generic;
using System.Text;

namespace OMSBlazor.Dto
{
    public class CreateOrderDto
    {
        public CreateOrderDto(int customerId, int employeeId, List<OrderDetailDto> orderDetails)
        {
            CustomerId = customerId;
            EmployeeId = employeeId;
            OrderDetails = orderDetails;
        }

        public int CustomerId { get; }

        public int EmployeeId { get; }

        public List<OrderDetailDto> OrderDetails { get; }
    }
}
