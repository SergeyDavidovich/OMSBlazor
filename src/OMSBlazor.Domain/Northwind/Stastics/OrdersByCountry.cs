using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Values;

namespace OMSBlazor.Northwind.Stastics
{
    public class OrdersByCountry : Entity<string>
    {
        private OrdersByCountry() { }

        public OrdersByCountry(string countryName)
        {
            CountryName = countryName;
        }

        public string CountryName { get; }
        public int OrdersCount { get; set; }
    }
}
