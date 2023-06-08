using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Values;

namespace OMSBlazor.Northwind.Common
{
    public class AddressObject : ValueObject
    {
        public AddressObject(
            string address, 
            string city, 
            string region, 
            string postalCode, 
            string country)
        {
            Address = address;
            City = city;
            Region = region;
            PostalCode = postalCode;
            Country = country;
        }

        public string Address { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Address;
            yield return City;
            yield return Region;
            yield return PostalCode;
            yield return Country;
        }
    }
}
