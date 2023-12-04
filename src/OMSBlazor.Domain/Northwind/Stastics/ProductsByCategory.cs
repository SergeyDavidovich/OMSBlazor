using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.Northwind.Stastics
{
    public class ProductsByCategory
    {
        public ProductsByCategory(string categoryName)
        {
            CategoryName = categoryName;
        }

        public string CategoryName { get; }
        public int ProductsCount { get; set; }
    }
}
