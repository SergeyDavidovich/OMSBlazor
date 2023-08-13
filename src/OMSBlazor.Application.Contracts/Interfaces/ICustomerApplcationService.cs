using OMSBlazor.Dto.Customer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.Interfaces
{
    public interface ICustomerApplcationService
    {
        public Task<List<CustomerDto>> GetCustomersAsync();
    }
}
