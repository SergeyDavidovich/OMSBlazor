using Microsoft.EntityFrameworkCore;
using OMSBlazor.HostModels;
using OMSBlazor.Northwind.CustomerAggregate;
using OMSBlazor.Northwind.EmployeeAggregate;
using OMSBlazor.Northwind.OrderAggregate;
using OMSBlazor.Northwind.ProductAggregate;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace OMSBlazor.EntityFrameworkCore;

[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class OMSBlazorDbContext :
    AbpDbContext<OMSBlazorDbContext>,
    ITenantManagementDbContext
{
    public DbSet<Order> Orders { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<CustomerDemographics> CustomerDemographics { get; set; }

    #region Entities from the modules

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public OMSBlazorDbContext(DbContextOptions<OMSBlazorDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.Entity<CustomerDemographics>(b =>
        {
            b.ToTable("CustomerDemographics");

            b.HasKey(x => x.CustomerTypeId);

            b.HasData(new CustomerDemographics() { CustomerTypeId = 1, CustomerDescription = "Lorem ipsum" });
        });
        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureOpenIddict();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(OMSBlazorConsts.DbTablePrefix + "YourEntities", OMSBlazorConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
        builder.Entity<OrderDetail>()
            .HasKey(nameof(OrderDetail.ProductId), nameof(OrderDetail.OrderId));

        builder.Entity<Order>()
            .Property(x => x.Id)
            .HasColumnName("OrderId");
        builder.Entity<Product>()
            .Property(x => x.Id)
            .HasColumnName("ProductId");
        builder.Entity<Customer>()
            .Property(x => x.Id)
            .HasColumnName("CustomerId");
        builder.Entity<Category>()
            .Property(x => x.Id)
            .HasColumnName("CategoryId");
        builder.Entity<Employee>()
            .Property(x => x.Id)
            .HasColumnName("EmployeeId");

        builder.Entity<Order>()
            .Ignore(x => x.ConcurrencyStamp)
            .Ignore(x=>x.ExtraProperties);
        builder.Entity<Product>()
            .Ignore(x => x.ConcurrencyStamp)
            .Ignore(x => x.ExtraProperties);
        builder.Entity<Employee>()
            .Ignore(x => x.ConcurrencyStamp)
            .Ignore(x => x.ExtraProperties);
        builder.Entity<Customer>()
            .Ignore(x => x.ConcurrencyStamp)
            .Ignore(x => x.ExtraProperties);
    }
}
