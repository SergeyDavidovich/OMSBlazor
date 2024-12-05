using StripeModule.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace StripeModule.Interfaces
{
    public interface IPaymentApplicationService : IApplicationService
    {
        Task<PaymentDto> CreateAsync(string orderId, decimal amount, Currency currency);

        Task<string> GetCheckoutUrl(double price, string orderId, string domain, string currency = "usd");
    }
}
