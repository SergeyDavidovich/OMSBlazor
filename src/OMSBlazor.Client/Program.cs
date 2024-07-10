using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using OMSBlazor.Client;
using OMSBlazor.Client.Pages.Dashboard.CustomerStastics;
using OMSBlazor.Client.Pages.Dashboard.EmployeeStastics;
using OMSBlazor.Client.Pages.Dashboard.OrderStastics;
using OMSBlazor.Client.Pages.Dashboard.ProductStastics;
using OMSBlazor.Client.Pages.Order.Create;
using OMSBlazor.Client.Pages.Order.Journal;
using ReactiveUI;
using Splat;

namespace OMSBlazor.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            // https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/configuration?view=aspnetcore-8.0#app-settings-configuration
            var http = new HttpClient() { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };

            builder.Services.AddScoped(sp => http);

            using var response = await http.GetAsync("appsettings.json");
            using var stream = await response.Content.ReadAsStreamAsync();

            builder.Configuration.AddJsonStream(stream);

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["BackendUrl"]) });
            builder.Services.AddMudServices();

            builder.Services.AddAuthorizationCore();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

            // We register view models on server too because server need them for pre-render
            // Please read this for further understanding - https://stackoverflow.com/a/78535224
            builder.Services.AddScoped<CustomerStasticsViewModel>();
            builder.Services.AddScoped<EmployeeStasticsViewModel>();
            builder.Services.AddScoped<OrderStasticsViewModel>();
            builder.Services.AddScoped<ProductStasticsViewModel>();
            builder.Services.AddScoped<JournalViewModel>();
            builder.Services.AddScoped<CreateViewModel>();

            await builder.Build().RunAsync();
        }
    }
}
