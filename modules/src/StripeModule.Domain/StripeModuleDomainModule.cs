using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace StripeModule;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(StripeModuleDomainSharedModule)
)]
public class StripeModuleDomainModule : AbpModule
{

}
