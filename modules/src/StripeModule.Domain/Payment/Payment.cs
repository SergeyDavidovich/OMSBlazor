using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace StripeModule.Payment
{
    public class Payment : Entity<Guid>
    {
        private Payment() { }

        public Payment(Guid id, object productId, Currency currency, decimal amount)
            : base(id)
        {
            ProductId = productId;
            Currency = currency;
            Amount = amount;
        }

        public Guid Id { get; }

        /// <summary>
        /// Id of the product/service for which this payment was done
        /// </summary>
        public object ProductId { get; }

        public Currency Currency { get; }

        public decimal Amount { get; }
    }
}
