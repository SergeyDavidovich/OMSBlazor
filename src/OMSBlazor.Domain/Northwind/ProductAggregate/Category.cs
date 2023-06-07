using OMSBlazor.Northwind.ProductAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Northwind.ProductAggregate
{
    public class Category : Entity<Guid>
    {
        public Category(Guid id, string name)
            : base(id) 
        {
            SetCategoryName(name);
        }

        public string CategoryName { get; private set; }

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
