using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Uow;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using StripeModule.EntityFrameworkCore;

namespace OMSBlazor.EntityFrameworkCore;

[DependsOn(
    typeof(OMSBlazorDomainModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqliteModule),
    typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(StripeModuleEntityFrameworkCoreModule)
    )]
public class OMSBlazorEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        OMSBlazorEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<OMSBlazorDbContext>(options =>
        {
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        context.Services.AddAbpDbContext<OMSBlazorIdentityDbContext>();

        //This configuration is needed for sqlite
        //https://github.com/abpframework/abp/issues/5661?ysclid=lhfcwt9oqb875559031
        Configure<AbpUnitOfWorkDefaultOptions>(options =>
        {
            options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
        });

        Configure<AbpDbContextOptions>(options =>
        {
            /* The main point to change your DBMS.
             * See also OMSBlazorMigrationsDbContextFactory for EF Core tooling. */
            options.UseSqlite();
        });

    }
}
