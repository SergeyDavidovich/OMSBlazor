using OMSBlazor.Dto.Employee;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OMSBlazor.Application.Contracts.Interfaces
{
    public interface IEmployeeApplicationService : IApplicationService
    {
        public Task<List<EmployeeDto>> GetEmployeesAsync();
    }
}
