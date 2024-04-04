using System;
using System.Collections.Generic;
using System.Linq;

namespace OMSBlazor.Blazor.Services
{
    public class InvoiceModel
    {
        public int InvoiceNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public string EmployeeFullName { get; set; }

        public string CustomerName { get; set; }

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public decimal TotalPrice
        {
            get
            {
                return Items.Sum(x => x.Quantity * (x.Price - x.Price * (x.Discount / 100)));
            }
        }
    }
}
