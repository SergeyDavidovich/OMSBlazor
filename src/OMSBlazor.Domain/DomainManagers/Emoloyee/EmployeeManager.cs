using OMSBlazor.Northwind.OrderAggregate;
using OMSBlazor.Northwind.OrderAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
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

        public async Task ThrowIfCannotDeleteAsync(int id)
        {
            if (!(await _employeeRepository.AnyAsync(x => x.Id == id)))
            {
                throw new EntityNotFoundException(typeof(Employee), id);
            }

            var dependentEmployee = await _employeeRepository.FirstOrDefaultAsync(x => x.ReportsTo == id);
            if (dependentEmployee is not null)
            {
                throw new DependentReporterExistException(id, dependentEmployee.Id);
            }
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
