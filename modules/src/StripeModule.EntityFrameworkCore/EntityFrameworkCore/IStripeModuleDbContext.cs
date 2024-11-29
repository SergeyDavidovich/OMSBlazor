using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace StripeModule.EntityFrameworkCore;

[ConnectionStringName(StripeModuleDbProperties.ConnectionStringName)]
public interface IStripeModuleDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
