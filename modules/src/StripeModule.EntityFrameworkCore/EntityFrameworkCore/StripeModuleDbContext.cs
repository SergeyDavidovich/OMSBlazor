using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace StripeModule.EntityFrameworkCore;

[ConnectionStringName(StripeModuleDbProperties.ConnectionStringName)]
public class StripeModuleDbContext : AbpDbContext<StripeModuleDbContext>, IStripeModuleDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public StripeModuleDbContext(DbContextOptions<StripeModuleDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureStripeModule();
    }
}
