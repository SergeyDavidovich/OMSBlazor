using StripeModule.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace StripeModule.Interfaces
{
    public interface IPaymentApplicationService : IApplicationService
    {
        Task<PaymentDto> CreateAsync(object productId, decimal amount, Currency currency);
    }
}
