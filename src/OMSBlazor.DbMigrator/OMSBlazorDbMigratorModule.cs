using OMSBlazor.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace OMSBlazor.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(OMSBlazorEntityFrameworkCoreModule),
    typeof(OMSBlazorApplicationContractsModule)
    )]
public class OMSBlazorDbMigratorModule : AbpModule
{

}
