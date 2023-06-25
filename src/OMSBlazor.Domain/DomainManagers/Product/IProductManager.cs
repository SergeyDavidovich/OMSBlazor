using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.DomainManagers.Product
{
    public interface IProductManager
    {
        public Task<Northwind.ProductAggregate.Product> Create(string name, Northwind.ProductAggregate.Category category);
    }
}
