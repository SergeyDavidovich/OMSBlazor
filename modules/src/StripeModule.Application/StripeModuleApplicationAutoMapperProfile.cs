using AutoMapper;
using StripeModule.DTOs;
using StripeModule.Payment;

namespace StripeModule;

public class StripeModuleApplicationAutoMapperProfile : Profile
{
    public StripeModuleApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Payment.Payment, PaymentDto>();
    }
}
