using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.NotificationSender.Signalr.Messages
{
    public class ProductUnitsInStockUpdatedMessage
    {
        public int ProductId { get; set; }

        public int NewUnitsInStockNumber { get; set; }
    }
}
