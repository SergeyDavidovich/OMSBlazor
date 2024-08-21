using Microsoft.AspNetCore.SignalR.Client;

namespace OMSBlazor.Client.Services.HubConnectionsService
{
    public interface IHubConnectionsService
    {
        public HubConnection ProductHubConnection { get; }

        public HubConnection DashboardHubConnection { get; }
    }
}
