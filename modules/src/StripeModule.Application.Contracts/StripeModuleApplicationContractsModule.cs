using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace StripeModule;

[DependsOn(
    typeof(StripeModuleDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class StripeModuleApplicationContractsModule : AbpModule
{

}
