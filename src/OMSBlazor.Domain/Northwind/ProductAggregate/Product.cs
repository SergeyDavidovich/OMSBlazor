using OMSBlazor.Northwind.ProductAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Northwind.ProductAggregate
{
    public class Product : AggregateRoot<int>
    {
        public Product(int id, string name, Category category) :
            base(id)
        {
            ProductName = name;
            Category = category;
        }

        public string ProductName { get; private set; }

        public Category Category { get; private set; }

        public string QuantityPerUnit { get; set; }

        public double UnitPrice { get; set; }

        public int UnitsInStock { get; set; }

        public int UnitsOnOrder { get; set; }

        public int ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public void SetProductName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new EmptyProductNameException();
            }

            ProductName = name;
        }
    }
}
