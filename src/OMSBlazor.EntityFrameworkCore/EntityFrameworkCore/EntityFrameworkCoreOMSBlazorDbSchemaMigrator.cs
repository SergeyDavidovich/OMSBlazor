using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OMSBlazor.Data;
using Volo.Abp.DependencyInjection;

namespace OMSBlazor.EntityFrameworkCore;

public class EntityFrameworkCoreOMSBlazorDbSchemaMigrator
    : IOMSBlazorDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreOMSBlazorDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the OMSBlazorDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<OMSBlazorDbContext>()
            .Database
            .MigrateAsync();

        await _serviceProvider
            .GetRequiredService<OMSBlazorIdentityDbContext>()
            .Database
            .MigrateAsync();
    }
}
