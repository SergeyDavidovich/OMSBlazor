// https://codejack.com/2022/12/deploying-abp-io-to-an-azure-appservice/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OMSBlazor.EntityFrameworkCore;
using OMSBlazor.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using Microsoft.AspNetCore.Hosting;
using Volo.Abp.OpenIddict;

using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Volo.Abp.AspNetCore.SignalR;
using OMSBlazor.NotificationSender;
using System.Threading.Tasks;
using OMSBlazor.Northwind.Stastics;
using Microsoft.EntityFrameworkCore;

namespace OMSBlazor;

[DependsOn(
    typeof(OMSBlazorHttpApiModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(OMSBlazorApplicationModule),
    typeof(OMSBlazorEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSignalRModule),
    typeof(OMSBlazorNotificationSenderModule)
)]
public class OMSBlazorHttpApiHostModule : AbpModule
{
    IWebHostEnvironment? hostingEnvironment;
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        hostingEnvironment = context.Services.GetHostingEnvironment();

        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("OMSBlazor");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });

        if (!hostingEnvironment.IsDevelopment())
        {
            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
            {
                options.AddDevelopmentEncryptionAndSigningCertificate = false;
            });

            PreConfigure<OpenIddictServerBuilder>(builder =>
            {
                // In production, it is recommended to use two RSA certificates, 
                // one for encryption, one for signing.
                builder.AddEncryptionCertificate(
                        GetEncryptionCertificate(hostingEnvironment, context.Services.GetConfiguration()));
                builder.AddSigningCertificate(
                        GetSigningCertificate(hostingEnvironment, context.Services.GetConfiguration()));
            });
        }

    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();


        ConfigureAuthentication(context);
        ConfigureBundles();
        ConfigureUrls(configuration);
        ConfigureConventionalControllers();
        ConfigureVirtualFileSystem(context);
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context, configuration);
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                BasicThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());

            options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
            options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
        });
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<OMSBlazorDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}OMSBlazor.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<OMSBlazorDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}OMSBlazor.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<OMSBlazorApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}OMSBlazor.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<OMSBlazorApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}OMSBlazor.Application"));
            });
        }
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(OMSBlazorApplicationModule).Assembly);
        });
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"],
            new Dictionary<string, string>
            {
                    {"OMSBlazor", "OMSBlazor API"}
            },
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "OMSBlazor API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray() ?? Array.Empty<string>())
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "OMSBlazor API");

            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            c.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            c.OAuthScopes("OMSBlazor");
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();

        await SeedStatistics(app.ApplicationServices);
    }

    private async Task SeedStatistics(IServiceProvider applicationServices)
    {
        var context = applicationServices.GetRequiredService<OMSBlazorDbContext>();

        if (!context.OrdersByCountries.Any())
        {
            var groupedOrders = context
                .Orders
                .GroupBy(order => context.Customers.Single(x => x.Id == order.CustomerId).Country);

            var ordersByCountries = await groupedOrders
                .Select(orderGroup => new OrdersByCountry(orderGroup.Key)
                {
                    OrdersCount = orderGroup.Count()
                })
                .ToListAsync();

            await context.OrdersByCountries.AddRangeAsync(ordersByCountries);
        }

        if (!context.SalesByCategories.Any())
        {
            var groupedOrderDetail = context
                .OrderDetails
                .GroupBy(orderDetail => context.Categories.Single(y=>y.Id== context.Products.Single(x=>x.Id==orderDetail.ProductId).CategoryId).CategoryName);

            var salesByCategories = await groupedOrderDetail
                .Select(orderDetailGroup => new SalesByCategory(orderDetailGroup.Key)
                {
                    Sales = (decimal)orderDetailGroup.Sum(orderDetail => orderDetail.Quantity * orderDetail.UnitPrice)
                })
                .ToListAsync();

            await context.SalesByCategories.AddRangeAsync(salesByCategories);
        }

        if (!context.Summaries.Any())
        {
            var overallSales = await context.OrderDetails.SumAsync(a => a.Quantity * a.UnitPrice);
            var ordersQuantity = await context.Orders.CountAsync();
            var groupedOrderDetails = context.OrderDetails.GroupBy(od => od.OrderId);
            var ordersChecks = await groupedOrderDetails.Select(god => new { Sales = god.Sum(a => a.Quantity * a.UnitPrice) }).ToListAsync();
            var maxCheck = ordersChecks.Max(a => a.Sales);
            var averageCheck = ordersChecks.Average(a => a.Sales);
            var minCheck = ordersChecks.Min(a => a.Sales);

            await context.Summaries.AddAsync(new Summary()
            {
                SummaryName = "OverallSales",
                SummaryValue = overallSales
            });
            await context.Summaries.AddAsync(new Summary()
            {
                SummaryName = "OrdersQuantity",
                SummaryValue = ordersQuantity
            });
            await context.Summaries.AddAsync(new Summary()
            {
                SummaryName = "MaxCheck",
                SummaryValue = maxCheck
            });
            await context.Summaries.AddAsync(new Summary()
            {
                SummaryName = "AverageCheck",
                SummaryValue = averageCheck
            });
            await context.Summaries.AddAsync(new Summary()
            {
                SummaryName = "MinCheck",
                SummaryValue = minCheck
            });
        }

        if (!context.SalesByCountries.Any())
        {
            var groupedOrderDetails = context
                .OrderDetails
                .GroupBy(orderDetail => context.Customers.Single(x => x.Id == (context.Orders.Single(y => y.Id == orderDetail.OrderId).CustomerId)).Country);

            var salesByCountries = await groupedOrderDetails
                .Select(orderDetailGroup => new SalesByCountry(orderDetailGroup.Key)
                {
                    Sales = (decimal)orderDetailGroup.Sum(orderDetail => orderDetail.Quantity * orderDetail.UnitPrice)
                })
                .ToListAsync();

            await context.SalesByCountries.AddRangeAsync(salesByCountries);
        }

        if (!context.CustomersByCountries.Any())
        {
            var groupedCustomers = context
                .Customers
                .GroupBy(customer => customer.Country);

            var customersByCountries = await groupedCustomers
                .Select(customerGroup => new CustomersByCountry(customerGroup.Key)
                {
                    CustomersCount = customerGroup.Count()
                })
                .ToListAsync();

            await context.CustomersByCountries.AddRangeAsync(customersByCountries);
        }

        if (!context.PurchasesByCustomers.Any())
        {
            var groupedOrderDetails = context
                .OrderDetails
                .GroupBy(orderDetail => context.Customers.Single(x => x.Id == (context.Orders.Single(y => y.Id == orderDetail.OrderId).CustomerId)).CompanyName);

            var purchasesByCustomers = await groupedOrderDetails
                .Select(orderDetailGroup => new PurchasesByCustomer(orderDetailGroup.Key)
                {
                    Purchases = (decimal)orderDetailGroup.Sum(orderDetail => orderDetail.Quantity * orderDetail.UnitPrice)
                })
                .ToListAsync();

            await context.PurchasesByCustomers.AddRangeAsync(purchasesByCustomers);
        }

        if (!context.SalesByEmployees.Any())
        {
            var groupedOrderDetails = context
                .OrderDetails
                .GroupBy(orderDetail => new 
                { 
                    Id = context.Employees.Single(x => x.Id == (context.Orders.Single(y => y.Id == orderDetail.OrderId).EmployeeId)).Id,
                    LastName = context.Employees.Single(x => x.Id == (context.Orders.Single(y => y.Id == orderDetail.OrderId).EmployeeId)).LastName
                });

            var salesByEmployees = await groupedOrderDetails
                .Select(orderDetailGroup => new SalesByEmployee(orderDetailGroup.Key.Id, orderDetailGroup.Key.LastName)
                {
                    Sales = (decimal)orderDetailGroup.Sum(orderDetail => orderDetail.Quantity * orderDetail.UnitPrice)
                })
                .ToListAsync();

            await context.SalesByEmployees.AddRangeAsync(salesByEmployees);
        }

        if (!context.ProductsByCategories.Any())
        {
            var productsByCategories = await context
                .Categories
                .Select(category => new ProductsByCategory(category.CategoryName)
                {
                    ProductsCount = context.Products.Where(x => x.CategoryId == category.Id).Count()
                })
                .ToListAsync();

            await context.ProductsByCategories.AddRangeAsync(productsByCategories);
        }

        await context.SaveChangesAsync();
    }

    private X509Certificate2 GetSigningCertificate(IWebHostEnvironment hostingEnv,
                            IConfiguration configuration)
    {
        var fileName = $"cert-signing.pfx";

        var passPhrase = configuration["MyAppCertificate:X590"];

        var file = Path.Combine(hostingEnv.ContentRootPath, fileName);
        if (File.Exists(file))
        {
            var created = File.GetCreationTime(file);
            var days = (DateTime.Now - created).TotalDays;
            if (days > 180)
                File.Delete(file);
            else
                return new X509Certificate2(file, passPhrase,
                             X509KeyStorageFlags.MachineKeySet);
        }

        // file doesn't exist or was deleted because it expired
        using var algorithm = RSA.Create(keySizeInBits: 2048);
        var subject = new X500DistinguishedName("CN=Fabricam Signing Certificate");
        var request = new CertificateRequest(subject, algorithm,
                            HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        request.CertificateExtensions.Add(new X509KeyUsageExtension(
                            X509KeyUsageFlags.DigitalSignature, critical: true));
        var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow,
                            DateTimeOffset.UtcNow.AddYears(2));
        File.WriteAllBytes(file, certificate.Export(X509ContentType.Pfx, string.Empty));
        return new X509Certificate2(file, passPhrase,
                            X509KeyStorageFlags.MachineKeySet);
    }

    private X509Certificate2 GetEncryptionCertificate(IWebHostEnvironment hostingEnv,
                                 IConfiguration configuration)
    {
        var fileName = $"cert-encryption.pfx";
        var passPhrase = configuration["MyAppCertificate:X590"];
        var file = Path.Combine(hostingEnv.ContentRootPath, fileName);
        if (File.Exists(file))
        {
            var created = File.GetCreationTime(file);
            var days = (DateTime.Now - created).TotalDays;
            if (days > 180)
                File.Delete(file);
            else
                return new X509Certificate2(file, passPhrase,
                                X509KeyStorageFlags.MachineKeySet);
        }

        // file doesn't exist or was deleted because it expired
        using var algorithm = RSA.Create(keySizeInBits: 2048);
        var subject = new X500DistinguishedName("CN=Fabricam Encryption Certificate");
        var request = new CertificateRequest(subject, algorithm,
                            HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        request.CertificateExtensions.Add(new X509KeyUsageExtension(
                            X509KeyUsageFlags.KeyEncipherment, critical: true));
        var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow,
                            DateTimeOffset.UtcNow.AddYears(2));
        File.WriteAllBytes(file, certificate.Export(X509ContentType.Pfx, string.Empty));
        return new X509Certificate2(file, passPhrase, X509KeyStorageFlags.MachineKeySet);
    }
}
