using OMSBlazor.Dto.Employee;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.Interfaces.Application.Contracts.Interfaces
{
    public interface IEmployeeApplicationService
    {
        public Task<List<EmployeeDto>> GetEmployeesAsync();
    }
}
