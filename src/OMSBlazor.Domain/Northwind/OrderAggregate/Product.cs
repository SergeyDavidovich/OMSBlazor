﻿using OMSBlazor.Northwind.OrderAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Northwind.OrderAggregate
{
    public class Product : Entity<int>
    {
        private Product()
        {

        }

        internal Product(int id, string name, int categoryId) :
            base(id)
        {
            ProductName = name;
            CategoryId = categoryId;
        }

        public string ProductName { get; private set; }

        public int CategoryId { get; private set; }

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
