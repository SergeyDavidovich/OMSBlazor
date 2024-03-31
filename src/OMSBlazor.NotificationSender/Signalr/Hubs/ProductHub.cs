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
        public async Task UpdateProductUnitsInStock(string excludeId, int productId, int quantity)
        {
            await Clients.AllExcept(excludeId).SendAsync("UpdateQuantity", productId, quantity);
        }

        public override Task OnConnectedAsync()
        {
            Logger.LogInformation($"New connection with id - {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Logger.LogInformation($"Closed connection with id - {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
