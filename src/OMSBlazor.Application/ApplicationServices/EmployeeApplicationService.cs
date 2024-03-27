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
using OMSBlazor.DomainManagers.Emoloyee;
using OMSBlazor.Northwind.OrderAggregate.Exceptions;
using Volo.Abp.Domain.Entities;

namespace OMSBlazor.Application.ApplicationServices
{
    public class EmployeeApplicationService : ApplicationService, IEmployeeApplicationService
    {
        private readonly IRepository<Employee, int> _employeeRepository;
        private readonly IEmployeeManager _employeeManager;

        public EmployeeApplicationService(IRepository<Employee, int> employeeRepository, IEmployeeManager employeeManager)
        {
            _employeeRepository = employeeRepository;
            _employeeManager = employeeManager;
        }

        public async Task CreateEmployeeAsync(CreateEmployeeDto employeeDto)
        {
            var employee = await _employeeManager.CreateAsync(employeeDto.FirstName, employeeDto.LastName);

            employee.Notes = employeeDto.Notes;
            employee.PhotoPath = employeeDto.PhotoPath;
            employee.HomePhone = employeeDto.HomePhone;
            employee.HireDate = employeeDto.HireDate;
            employee.BirthDate = employeeDto.BirthDate;
            employee.Address = employeeDto.Address;
            employee.City = employeeDto.City;
            employee.Country = employeeDto.Country;
            employee.Extension = employeeDto.Extension;
            employee.TitleOfCourtesy = employeeDto.TitleOfCoursery;
            employee.ReportsTo = employeeDto.ReportsTo;
            employee.Title = employeeDto.Title;
            employee.Region = employeeDto.Region;
            employee.PostalCode = employeeDto.PostalCode;

            await _employeeRepository.InsertAsync(employee);
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            await _employeeManager.ThrowIfCannotDeleteAsync(id);

            await _employeeRepository.DeleteAsync(id);
        }

        public async Task<EmployeeDto> GetEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.GetAsync(id);

            var employeeDto = ObjectMapper.Map<Employee, EmployeeDto>(employee);

            return employeeDto;
        }

        public async Task<List<EmployeeDto>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetQueryableAsync();

            var employeeDtos = employees.Select(x => ObjectMapper.Map<Employee, EmployeeDto>(x)).ToList();

            return employeeDtos;
        }

        public async Task UpdateEmployeeAsync(int id, UpdateEmployeeDto employeeDto)
        {
            var employee = await _employeeRepository.SingleOrDefaultAsync(x => x.Id == id);

            if (employee is null)
            {
                throw new EntityNotFoundException(typeof(Employee), id);
            }

            employee.SetFirstName(employeeDto.FirstName);
            employee.SetLastName(employeeDto.LastName);
            employee.BirthDate = employeeDto.BirthDate;
            employee.HireDate = employeeDto.HireDate;
            employee.PhotoPath = employeeDto.PhotoPath;
            employee.Address = employeeDto.Address;
            employee.City = employeeDto.City;
            employee.Country = employeeDto.Country;
            employee.TitleOfCourtesy = employeeDto.TitleOfCoursery;
            employee.Notes = employeeDto.Notes;
            employee.PostalCode = employeeDto.PostalCode;
            employee.Title = employeeDto.Title;
            employee.Region = employeeDto.Region;

            await _employeeRepository.UpdateAsync(employee);
        }
    }
}
