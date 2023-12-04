using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.Northwind.Stastics
{
    public class SalesByCountry
    {
        public SalesByCountry(string countryName)
        {
            CountryName = countryName;
        }

        public string CountryName { get; }
        public decimal Sales { get; set; }
    }
}
