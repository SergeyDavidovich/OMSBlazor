using System;
using System.Collections.Generic;
using System.Text;

namespace OMSBlazor.Dto.Order
{
    public class ShipDataDto
    {
        public string ShipName { get; internal set; }

        public string ShipAddress { get; internal set; }

        public string ShipRegion { get; internal set; }

        public string ShipCity { get; internal set; }

        public string ShipPostalCode { get; internal set; }

        public string ShipCountry { get; internal set; }
    }
}
