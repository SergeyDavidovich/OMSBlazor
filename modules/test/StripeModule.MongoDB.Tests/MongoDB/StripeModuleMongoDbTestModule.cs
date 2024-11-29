using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace StripeModule.MongoDB;

[DependsOn(
    typeof(StripeModuleApplicationTestModule),
    typeof(StripeModuleMongoDbModule)
)]
public class StripeModuleMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}
