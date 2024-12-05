using Microsoft.EntityFrameworkCore;
using StripeModule.Payment;
using Volo.Abp;

namespace StripeModule.EntityFrameworkCore;

public static class StripeModuleDbContextModelCreatingExtensions
{
    public static void ConfigureStripeModule(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(StripeModuleDbProperties.DbTablePrefix + "Questions", StripeModuleDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */

        builder.Entity<Payment.Payment>()
            .HasKey(x => x.Id);
    }
}
