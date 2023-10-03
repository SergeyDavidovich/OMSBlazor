using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.DomainManagers.Category
{
    public interface ICategoryManager
    {
        public Task<Northwind.OrderAggregate.Category> CreateAsync(string name);
    }
}
