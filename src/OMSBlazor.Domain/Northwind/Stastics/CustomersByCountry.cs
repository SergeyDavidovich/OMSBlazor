﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace OMSBlazor.Northwind.Stastics
{
    public class CustomersByCountry : Entity<int>, IMultiTenant
    {
        private CustomersByCountry() { }

        public CustomersByCountry(string countryName)
        {
            Key = countryName;
        }

        public string Key { get; set; }

        public int Value { get; set; }

        public Guid? TenantId { get; set; }
    }
}
