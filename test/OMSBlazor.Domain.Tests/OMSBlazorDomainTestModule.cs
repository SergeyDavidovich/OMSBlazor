using OMSBlazor.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace OMSBlazor;

[DependsOn(
    typeof(OMSBlazorEntityFrameworkCoreTestModule)
    )]
public class OMSBlazorDomainTestModule : AbpModule
{

}
