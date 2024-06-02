using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace OMSBlazor.Northwind.Stastics
{
    public class SalesByEmployee : Entity<int>, IMultiTenant
    {
        private SalesByEmployee() { }

        public SalesByEmployee(int key, string lastName)
        {
            Key = key;
            LastName = lastName;
        }
        
        public int Key { get; set; }

        public string LastName { get; private set; }

        public decimal Value { get; set; }

        public Guid? TenantId { get; set; }
    }
}
