using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace OMSBlazor.Northwind.Stastics
{
    public class Summary : Entity<int>, IMultiTenant
    {
        public Summary()
        {
            
        }

        public string Key { get; set; }

        public double Value { get; set; }

        public Guid? TenantId { get; set; }
    }
}
