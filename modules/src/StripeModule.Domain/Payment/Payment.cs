using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace StripeModule.Payment
{
    public class Payment : Entity<Guid>
    {
        private Payment() { }

        public Payment(Guid id, int orderId, Currency currency, decimal amount)
            : base(id)
        {
            OrderId = orderId;
            Currency = currency;
            Amount = amount;
        }

        /// <summary>
        /// Id of the product/service for which this payment was done
        /// </summary>
        public int OrderId { get; private set; }

        public Currency Currency { get; private set; }

        public decimal Amount { get; private set; }
    }
}
