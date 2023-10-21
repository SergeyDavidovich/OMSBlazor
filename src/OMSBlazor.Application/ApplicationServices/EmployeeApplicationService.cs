using OMSBlazor.Dto.Employee;
using OMSBlazor.Application.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using OMSBlazor.Northwind.OrderAggregate;

namespace OMSBlazor.Application.ApplicationServices
{
    public class EmployeeApplicationService : ApplicationService, IEmployeeApplicationService
    {
        private readonly IRepository<Employee, int> _employeeRepository;

        public EmployeeApplicationService(IRepository<Employee, int> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<List<EmployeeDto>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetQueryableAsync();

            var employeeDtos = employees.Select(x => ObjectMapper.Map<Employee, EmployeeDto>(x)).ToList();

            return employeeDtos;
        }
    }
}
