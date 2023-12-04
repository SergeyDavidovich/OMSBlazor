using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.Northwind.Stastics
{
    public class OrdersByCountry
    {
        public OrdersByCountry(string countryName)
        {
            CountryName = countryName;
        }

        public string CountryName { get; }
        public int OrdersCount { get; set; }
    }
}
