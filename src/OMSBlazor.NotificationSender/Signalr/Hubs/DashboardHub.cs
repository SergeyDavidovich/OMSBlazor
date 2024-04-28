using Microsoft.AspNetCore.SignalR;
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
    }
}
