using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace StripeModule;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(StripeModuleHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class StripeModuleConsoleApiClientModule : AbpModule
{

}
