﻿using OMSBlazor.Northwind.OrderAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Northwind.OrderAggregate
{
    public class Category : Entity<int>
    {
        private Category()
        {

        }

        internal Category(int id, string name)
            : base(id)
        {
            SetCategoryName(name);
        }

        public string? CategoryName { get; private set; }

        public string? Description { get; set; }

        public string? Picture { get; set; }

        public void SetCategoryName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new EmptyCategoryNameException();
            }

            CategoryName = name;
        }
    }
}
