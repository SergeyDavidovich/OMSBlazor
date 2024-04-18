using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Northwind.Stastics
{
    public class ProductsByCategory : Entity<string>
    {
        private ProductsByCategory() { }

        public ProductsByCategory(string categoryName)
        {
            CategoryName = categoryName;
        }

        public string CategoryName { get; }
        public int ProductsCount { get; set; }
    }
}
