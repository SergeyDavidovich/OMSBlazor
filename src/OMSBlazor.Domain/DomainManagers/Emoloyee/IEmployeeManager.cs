using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.DomainManagers.Emoloyee
{
    public interface IEmployeeManager
    {
        Task<Northwind.OrderAggregate.Employee> CreateAsync(string firstName, string lastName);

        Task ThrowIfCannotDeleteAsync(int id);
    }
}
