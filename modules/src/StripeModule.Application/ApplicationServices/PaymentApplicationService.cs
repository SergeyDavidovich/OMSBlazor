using Stripe.Checkout;
using StripeModule.DomainManagers;
using StripeModule.DTOs;
using StripeModule.EventTransferObjects;
using StripeModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;

namespace StripeModule.ApplicationServices
{
    public class PaymentApplicationService : ApplicationService, IPaymentApplicationService
    {
        private readonly IRepository<Payment.Payment, Guid> _repository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IDistributedEventBus _distributedEventBus;

        public PaymentApplicationService(
            IRepository<Payment.Payment, Guid> repository, 
            IGuidGenerator guidGenerator, 
            IDistributedEventBus distributedEventBus)
        {
            _repository = repository;
            _guidGenerator = guidGenerator;
            _distributedEventBus = distributedEventBus;
        }

        public async Task<PaymentDto> CreateAsync(object productId, decimal amount, Currency currency)
        {
            var payment = new Payment.Payment(_guidGenerator.Create(), productId, currency, amount);

            var paymentEntity = await _repository.InsertAsync(payment);

            await _repository.InsertAsync(paymentEntity);

            await _distributedEventBus.PublishAsync(new PaymentCreatedEto
            {
                PaymentId = paymentEntity.Id,
                ProductId = paymentEntity.ProductId
            });

            return ObjectMapper.Map<Payment.Payment, PaymentDto>(paymentEntity);
        }

        public async Task<string> GetCheckoutUrl(double price, string orderId, string domain, string currency = "usd")
        {
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new()
                    {
                        PriceData = new()
                        {
                            UnitAmount = Convert.ToInt16(price) * 100, // цена в центах (например, £49 = 4900)
                            Currency = currency,
                            ProductData = new()
                            {
                                Name = $"Order#{orderId??"111"}",
                            }
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = domain + $"/OrderComplete/{orderId}/{currency}/{price}",
                CancelUrl = domain + "/OrderAbandoned"
            };
            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session.Url;
        }
    }
}
