using System;
using System.Collections.Generic;
using System.Text;

namespace StripeModule.DTOs
{
    public class PaymentDto
    {
        public Guid Id { get; set; }

        public int OrderId { get; set; }

        public Currency Currency { get; set; }

        public decimal Amount { get; set; }
    }
}
