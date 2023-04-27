using Volo.Abp.Modularity;

namespace OMSBlazor;

[DependsOn(
    typeof(OMSBlazorApplicationModule),
    typeof(OMSBlazorDomainTestModule)
    )]
public class OMSBlazorApplicationTestModule : AbpModule
{

}
