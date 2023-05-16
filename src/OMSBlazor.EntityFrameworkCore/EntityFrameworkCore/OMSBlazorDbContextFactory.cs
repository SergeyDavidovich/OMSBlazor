using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace OMSBlazor.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class OMSBlazorDbContextFactory : IDesignTimeDbContextFactory<OMSBlazorDbContext>
{
    public OMSBlazorDbContext CreateDbContext(string[] args)
    {
        OMSBlazorEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<OMSBlazorDbContext>()
            .UseSqlite(configuration.GetConnectionString("Default"));

        return new OMSBlazorDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../OMSBlazor.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
