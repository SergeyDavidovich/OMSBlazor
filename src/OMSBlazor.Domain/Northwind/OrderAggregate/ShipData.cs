using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Values;

namespace OMSBlazor.Northwind.OrderAggregate
{
    public class ShipData : ValueObject
    {
        public ShipData(
            string shipName, 
            string shipAddress, 
            string shipRegion, 
            string shipPostalCode, 
            string shipCountry,
            string shipCity) 
        {
            ShipName = shipName;
            ShipAddress = shipAddress;
            ShipRegion = shipRegion;
            ShipPostalCode = shipPostalCode;
            ShipCountry = shipCountry;
            ShipCity = shipCity;
        }

        public string ShipName { get; internal set; }

        public string ShipAddress { get; internal set; }

        public string ShipRegion { get; internal set; }

        public string ShipCity { get; internal set; }

        public string ShipPostalCode { get; internal set; }

        public string ShipCountry { get; internal set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ShipName;
            yield return ShipAddress;
            yield return ShipRegion;
            yield return ShipPostalCode;
            yield return ShipCountry;
            yield return ShipCity;
        }
    }
}
