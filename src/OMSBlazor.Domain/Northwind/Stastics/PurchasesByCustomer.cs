using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Northwind.Stastics
{
    public class PurchasesByCustomer : Entity<string>
    {
        private PurchasesByCustomer() { }

        public PurchasesByCustomer(string companyName)
        {
            CompanyName = companyName;
        }

        public string CompanyName { get; }
        public decimal Purchases { get; set; }
    }
}
