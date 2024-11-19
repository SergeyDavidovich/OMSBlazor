using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StripeModule.Backend.Model
{
    public class Payment
    {
        public Payment(Guid productId)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
        }

        public Guid Id { get; set; }

        /// <summary>
        /// Id of the product/service for which this payment was done
        /// </summary>
        public Guid ProductId { get; set; }
    }
}
