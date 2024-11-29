using Volo.Abp.Modularity;

namespace StripeModule;

[DependsOn(
    typeof(StripeModuleApplicationModule),
    typeof(StripeModuleDomainTestModule)
    )]
public class StripeModuleApplicationTestModule : AbpModule
{

}
