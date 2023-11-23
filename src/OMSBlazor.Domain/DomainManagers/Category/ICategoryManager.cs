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

        public Task ThrowIfCannotDeleteAsync(int id);

        public Task<Northwind.OrderAggregate.Category> UpdateAsync(int id, string name);
    }
}
