using OMSBlazor.Interfaces.ApplicationServices;
using StripeModule.EventTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EventBus.Distributed;

namespace OMSBlazor.EventHandlers
{
    public class PaymentCreatedEventHandler : IDistributedEventHandler<PaymentCreatedEto>
    {
        private readonly IOrderApplicationService _orderApplicationService;

        public PaymentCreatedEventHandler(IOrderApplicationService orderApplicationService)
        {
            _orderApplicationService = orderApplicationService;
        }

        public async Task HandleEventAsync(PaymentCreatedEto eventData)
        {
            await _orderApplicationService.SetPaymentId((int)eventData.ProductId, eventData.PaymentId);
        }
    }
}
