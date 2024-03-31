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
    public class ProductHub : AbpHub
    {
        public async Task UpdateProductUnitsInStock(string id, int productId, int quantity)
        {
            Logger.LogInformation($"Sending from connection with id - {id}");
            await Clients.All.SendAsync("UpdateQuantity", productId, quantity);
        }

        public override Task OnConnectedAsync()
        {
            Logger.LogInformation($"Connected with {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Logger.LogInformation($"Connected with {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
