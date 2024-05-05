using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.SignalR;

namespace OMSBlazor.NotificationSender.Signalr.Hubs
{
    public class DashboardHub : AbpHub
    {
        public async Task UpdateDashboard()
        {
            await Clients.All.SendAsync("UpdateDashboard");
        }

        public override Task OnConnectedAsync()
        {
            Logger.LogInformation("Connected to dashboard hub");
            return base.OnConnectedAsync();
        }
    }
}
