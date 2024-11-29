using Volo.Abp.Modularity;

namespace StripeModule;

[DependsOn(
    typeof(StripeModuleDomainModule),
    typeof(StripeModuleTestBaseModule)
)]
public class StripeModuleDomainTestModule : AbpModule
{

}
