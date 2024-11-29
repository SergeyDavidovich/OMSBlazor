using StripeModule.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace StripeModule;

public abstract class StripeModuleController : AbpControllerBase
{
    protected StripeModuleController()
    {
        LocalizationResource = typeof(StripeModuleResource);
    }
}
