using OMSBlazor.Northwind.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace OMSBlazor.DomainManagers.Emoloyee
{
    public class EmployeeManager : DomainService, IEmployeeManager
    {
        private readonly IRepository<Employee, int> _employeeRepository;

        public EmployeeManager(IRepository<Employee, int> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Employee> CreateAsync(string firstName, string lastName)
        {
            var lastEmployee = await _employeeRepository.LastAsync();

            var id = lastEmployee.Id + 1;

            var employee = new Employee(id, firstName, lastName);

            return employee;
        }
    }
}
