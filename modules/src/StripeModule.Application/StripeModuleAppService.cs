using StripeModule.Localization;
using Volo.Abp.Application.Services;

namespace StripeModule;

public abstract class StripeModuleAppService : ApplicationService
{
    protected StripeModuleAppService()
    {
        LocalizationResource = typeof(StripeModuleResource);
        ObjectMapperContext = typeof(StripeModuleApplicationModule);
    }
}
