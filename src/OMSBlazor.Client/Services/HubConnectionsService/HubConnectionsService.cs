using Microsoft.AspNetCore.SignalR.Client;

namespace OMSBlazor.Client.Services.HubConnectionsService
{
    public class HubConnectionsService : IHubConnectionsService
    {
        public HubConnectionsService(IConfiguration configuration)
        {
            ProductHubConnection = new HubConnectionBuilder()
                .WithUrl($"{configuration["BackendUrl"]}signalr-hubs/product")
                .Build();
            DashboardHubConnection = new HubConnectionBuilder()
                .WithUrl($"{configuration["BackendUrl"]}signalr-hubs/dashboard")
                .Build();
        }

        public HubConnection ProductHubConnection { get; private set; }

        public HubConnection DashboardHubConnection { get; private set; }
    }
}
