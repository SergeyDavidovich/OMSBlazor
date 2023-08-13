using OMSBlazor.Dto.Customer;
using OMSBlazor.Interfaces;
using OMSBlazor.Northwind.CustomerAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OMSBlazor.ApplicationServices
{
    public class CustomerApplicationService : ApplicationService, ICustomerApplcationService
    {
        private readonly IRepository<Customer, int> _customerRepository;

        public CustomerApplicationService(IRepository<Customer, int> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<CustomerDto>> GetCustomersAsync()
        {
            var customers = await _customerRepository.GetQueryableAsync();

            var customerDtos = customers.Select(x => ObjectMapper.Map<Customer, CustomerDto>(x)).ToList();

            return customerDtos;
        }
    }
}
