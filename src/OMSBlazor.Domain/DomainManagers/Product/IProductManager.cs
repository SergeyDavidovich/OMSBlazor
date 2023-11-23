using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.DomainManagers.Product
{
    public interface IProductManager
    {
        public Task<Northwind.OrderAggregate.Product> CreateAsync(string name, int categoryId);

        public Task ThrowIfCannotDeleteAsync(int id);

        public Task<Northwind.OrderAggregate.Product> UpdateAsync(int id, string name);
    }
}
