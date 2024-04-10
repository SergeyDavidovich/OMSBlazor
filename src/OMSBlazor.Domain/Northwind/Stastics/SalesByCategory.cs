using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Northwind.Stastics
{
    public class SalesByCategory : Entity<string>
    {
        private SalesByCategory() { }

        public SalesByCategory(string categoryName)
        {
            CategoryName = categoryName;
        }

        public string CategoryName { get; }
        public decimal Sales { get; set; }
    }
}
