using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EventBus;

namespace StripeModule.EventTransferObjects
{
    [EventName("StripeModule.PaymentCreated")]
    public class PaymentCreatedEto
    {
        public Guid PaymentId { get; set; }

        public object ProductId { get; set; }
    }
}
