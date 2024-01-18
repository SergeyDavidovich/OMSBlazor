using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.Northwind.Stastics
{
    public class PurchasesByCustomer
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
