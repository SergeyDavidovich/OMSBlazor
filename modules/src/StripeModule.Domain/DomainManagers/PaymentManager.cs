using StripeModule.Payment;
using StripeModule.Payment.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace StripeModule.DomainManagers
{
    public class PaymentManagers: DomainService, IPaymentManager
    {
    }
}
