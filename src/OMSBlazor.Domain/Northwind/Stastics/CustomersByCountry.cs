using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Northwind.Stastics
{
    public class CustomersByCountry : Entity<string>
    {
        private CustomersByCountry() { }

        public CustomersByCountry(string countryName)
        {
            CountryName = countryName;
        }

        public string CountryName { get; }

        public int CustomersCount { get; set; }
    }
}
