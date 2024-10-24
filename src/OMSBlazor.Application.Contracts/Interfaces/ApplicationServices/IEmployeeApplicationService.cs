﻿using OMSBlazor.Dto.Employee;
using OMSBlazor.Dto.Employee.Stastics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OMSBlazor.Interfaces.ApplicationServices
{
    public interface IEmployeeApplicationService : IApplicationService
    {
        public Task<List<EmployeeDto>> GetEmployeesAsync();

        public Task<EmployeeDto> GetEmployeeAsync(int id);

        public Task DeleteEmployeeAsync(int id);

        public Task CreateEmployeeAsync(CreateEmployeeDto employeeDto);

        public Task UpdateEmployeeAsync(int id, UpdateEmployeeDto employeeDto);

        public Task<IEnumerable<SalesByEmployeeDto>> GetSalesByEmployees();
    }
}
