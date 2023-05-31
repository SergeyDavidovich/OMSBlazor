using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Northwind.EmployeeAggregate
{
    public class Employee : AggregateRoot<Guid>
    {
        public Employee(Guid id)
            :base(id)
        {
            
        }
    }
}
