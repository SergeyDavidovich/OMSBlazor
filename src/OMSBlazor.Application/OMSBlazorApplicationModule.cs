using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using OMSBlazor.Interfaces.Services;
using OMSBlazor.Services;
using QuestPDF.Infrastructure;
using StripeModule;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace OMSBlazor;

[DependsOn(
    typeof(OMSBlazorDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(OMSBlazorApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(StripeModuleApplicationModule)
    )]
public class OMSBlazorApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<OMSBlazorApplicationModule>();
        });

        context.Services.AddTransient<IStasticsRecalculator, StasticsRecalculator>();
    }
}
