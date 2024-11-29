using Localization.Resources.AbpUi;
using StripeModule.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace StripeModule;

[DependsOn(
    typeof(StripeModuleApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class StripeModuleHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(StripeModuleHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<StripeModuleResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
