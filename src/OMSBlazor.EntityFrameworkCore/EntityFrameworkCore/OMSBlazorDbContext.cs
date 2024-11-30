using Microsoft.EntityFrameworkCore;
using OMSBlazor.HostModels;
using OMSBlazor.Northwind.OrderAggregate;
using OMSBlazor.Northwind.Stastics;
using StripeModule.EntityFrameworkCore;
using StripeModule.Payment;
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
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace OMSBlazor.EntityFrameworkCore;

[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class OMSBlazorDbContext :
    AbpDbContext<OMSBlazorDbContext>,
    ITenantManagementDbContext,
    ISettingManagementDbContext
{
    public DbSet<Order> Orders { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<OrderDetail> OrderDetails { get; set; }

    public DbSet<CustomerDemographics> CustomerDemographics { get; set; }

    public DbSet<Payment> Payments { get; set; }

    #region Stastics
    public DbSet<CustomersByCountry> CustomersByCountries { get; set; }

    public DbSet<OrdersByCountry> OrdersByCountries { get; set; }

    public DbSet<ProductsByCategory> ProductsByCategories { get; set; }

    public DbSet<PurchasesByCustomer> PurchasesByCustomers { get; set; }

    public DbSet<SalesByCategory> SalesByCategories { get; set; }

    public DbSet<SalesByCountry> SalesByCountries { get; set; }

    public DbSet<SalesByEmployee> SalesByEmployees { get; set; }

    public DbSet<Summary> Summaries { get; set; }
    #endregion

    #region Entities from the modules

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    public DbSet<Setting> Settings { get; set; }

    public DbSet<SettingDefinitionRecord> SettingDefinitionRecords { get; set; }

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
            .HasKey(x => new { x.OrderId, x.ProductId });

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
            .Ignore(x => x.ExtraProperties);

        builder.Entity<CustomersByCountry>()
            .HasKey(x => x.Id);
        builder.Entity<CustomersByCountry>()
            .HasIndex(x => new { x.Key, x.TenantId })
            .IsUnique();

        builder.Entity<PurchasesByCustomer>()
            .HasKey(x => x.Id);
        builder.Entity<PurchasesByCustomer>()
            .HasIndex(x => new { x.Key, x.TenantId })
            .IsUnique();

        builder.Entity<OrdersByCountry>()
            .HasKey(x => x.Id);
        builder.Entity<OrdersByCountry>()
            .HasIndex(x => new { x.Key, x.TenantId })
            .IsUnique();

        builder.Entity<ProductsByCategory>()
            .HasKey(x => x.Id);
        builder.Entity<ProductsByCategory>()
            .HasIndex(x => new { x.Key, x.TenantId })
            .IsUnique();

        builder.Entity<SalesByCategory>()
            .HasKey(x => x.Id);
        builder.Entity<SalesByCategory>()
            .HasIndex(x => new { x.Key, x.TenantId })
            .IsUnique();

        builder.Entity<SalesByCountry>()
            .HasKey(x => x.Id);
        builder.Entity<SalesByCountry>()
            .HasIndex(x => new { x.Key, x.TenantId })
            .IsUnique();

        builder.Entity<SalesByEmployee>()
            .HasKey(x => x.Id);
        builder.Entity<SalesByEmployee>()
            .HasIndex(x => new { x.Key, x.TenantId })
            .IsUnique();

        builder.Entity<Summary>()
            .HasKey(x => x.Id);
        builder.Entity<Summary>()
            .HasIndex(x => new { x.Key, x.TenantId })
            .IsUnique();

        builder.ConfigureStripeModule();
    }
}
