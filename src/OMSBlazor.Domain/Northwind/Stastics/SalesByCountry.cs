using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Northwind.Stastics
{
    public class SalesByCountry : Entity<string>
    {
        private SalesByCountry() { }

        public SalesByCountry(string countryName)
        {
            CountryName = countryName;
        }

        public string CountryName { get; }
        public decimal Sales { get; set; }
    }
}
