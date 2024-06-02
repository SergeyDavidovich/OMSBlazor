using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace OMSBlazor.Northwind.Stastics
{
    public class SalesByCountry : Entity<int>, IMultiTenant
    {
        private SalesByCountry() { }

        public SalesByCountry(string countryName)
        {
            Key = countryName;
        }

        public string Key { get; set; }

        public decimal Value { get; set; }

        public Guid? TenantId { get; set; }
    }
}
