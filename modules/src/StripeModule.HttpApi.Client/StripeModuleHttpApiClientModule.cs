using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace StripeModule;

[DependsOn(
    typeof(StripeModuleApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class StripeModuleHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(StripeModuleApplicationContractsModule).Assembly,
            StripeModuleRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<StripeModuleHttpApiClientModule>();
        });

    }
}
