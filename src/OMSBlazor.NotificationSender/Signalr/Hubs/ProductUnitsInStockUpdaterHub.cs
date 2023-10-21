using Microsoft.AspNetCore.SignalR;
using OMSBlazor.NotificationSender.Signalr.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.SignalR;

namespace OMSBlazor.NotificationSender.Signalr.Hubs
{
    public class ProductUnitsInStockUpdaterHub : AbpHub
    {
        public async Task UpdateProductUnitsInStock(ProductUnitsInStockUpdatedMessage message)
        {
            await Clients.All.SendAsync(Constants.ProductUnitsInStockUpdatedHandlerName, message);
        }
    }
}
