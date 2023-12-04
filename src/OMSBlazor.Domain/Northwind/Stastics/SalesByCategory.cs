using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.Northwind.Stastics
{
    public class SalesByCategory
    {
        public SalesByCategory(string categoryName)
        {
            CategoryName = categoryName;
        }

        public string CategoryName { get; }
        public decimal Sales { get; set; }
    }
}
