using OMSBlazor.Dto.Customer;
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
    public class CustomerApplicationService : ApplicationService, ICustomerApplcationService
    {
        private readonly IRepository<Customer, string> _customerRepository;

        public CustomerApplicationService(IRepository<Customer, string> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<CustomerDto>> GetCustomersAsync()
        {
            var customers = await _customerRepository.GetListAsync();

            var customerDtos = customers.Select(x => ObjectMapper.Map<Customer, CustomerDto>(x)).ToList();

            return customerDtos;
        }
    }
}
