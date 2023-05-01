using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.EntityFrameworkCore
{
    public class OMSBlazorIdentityDbContextFactory :
        IDesignTimeDbContextFactory<OMSBlazorIdentityDbContext>
    {
        public OMSBlazorIdentityDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();
            var builder = new DbContextOptionsBuilder<OMSBlazorIdentityDbContext>()
                .UseSqlite(configuration.GetConnectionString("AbpIdentity"));
            return new OMSBlazorIdentityDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../OMSBlazor.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
