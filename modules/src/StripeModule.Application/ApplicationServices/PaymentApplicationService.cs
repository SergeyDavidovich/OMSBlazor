using StripeModule.DomainManagers;
using StripeModule.DTOs;
using StripeModule.Interfaces;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace StripeModule.ApplicationServices
{
    public class PaymentApplicationService : ApplicationService, IPaymentApplicationService
    {
        private readonly IRepository<Payment.Payment, Guid> _repository;
        private readonly IGuidGenerator _guidGenerator;

        public PaymentApplicationService(IRepository<Payment.Payment, Guid> repository, IGuidGenerator guidGenerator)
        {
            _repository = repository;
            _guidGenerator = guidGenerator;
        }

        public async Task<PaymentDto> CreateAsync(object productId, decimal amount, Currency currency)
        {
            var payment = new Payment.Payment(_guidGenerator.Create(), productId, currency, amount);

            var paymentEntity = await _repository.InsertAsync(payment);

            await _repository.InsertAsync(paymentEntity);

            return ObjectMapper.Map<Payment.Payment, PaymentDto>(paymentEntity);
        }
    }
}
