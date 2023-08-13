using OMSBlazor.Northwind.ProductAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.DomainManagers.Category
{
    public interface ICategoryManager
    {
        public Task<Northwind.ProductAggregate.Category> CreateAsync(string name);
    }
}
