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
using OMSBlazor.DomainManagers.Customer;
using OMSBlazor.Northwind.OrderAggregate.Exceptions;
using Volo.Abp.Domain.Entities;
using OMSBlazor.Dto.Customer.Stastics;
using OMSBlazor.Northwind.Stastics;

namespace OMSBlazor.Application.ApplicationServices
{
    public class CustomerApplicationService : ApplicationService, ICustomerApplcationService
    {
        private readonly IRepository<Customer, string> _customerRepository;
        private readonly IRepository<CustomersByCountry, string> _customersByCountryRepository;
        private readonly IRepository<PurchasesByCustomer, string> _purchasesByCustomerRepository;
        private readonly ICustomerManager _customerManager;

        public CustomerApplicationService(
            IRepository<Customer, string> customerRepository,
            ICustomerManager customerManager,
            IRepository<CustomersByCountry, string> customersByCountryRepository,
            IRepository<PurchasesByCustomer, string> purchasesByCustomerRepository)
        {
            _customerRepository = customerRepository;
            _customerManager = customerManager;
            _customersByCountryRepository = customersByCountryRepository;
            _purchasesByCustomerRepository = purchasesByCustomerRepository;
        }

        public async Task CreateCustomerAsync(CreateCustomerDto customerDto)
        {
            var customer = await _customerManager.CreateAsync(customerDto.CompanyName);
            customer.ContactName = customerDto.ContactName;
            customer.Address = customerDto.Address;
            customer.City = customerDto.City;
            customer.PostalCode = customerDto.PostalCode;
            customer.Country = customerDto.Country;
            customer.Phone = customerDto.Phone;
            customer.Fax = customerDto.Fax;
            customer.Region = customerDto.Region;

            await _customerRepository.InsertAsync(customer);
        }

        public async Task DeleteCustomerAsync(string customerId)
        {
            await _customerManager.ThrowIfCannotDeleteAsync(customerId);

            await _customerRepository.DeleteAsync(customerId);
        }

        public async Task<CustomerDto> GetCustomerAsync(string customerId)
        {
            var customer = await _customerRepository.GetAsync(customerId);
            var customerDto = ObjectMapper.Map<Customer,  CustomerDto>(customer);

            return customerDto;
        }

        public async Task<List<CustomerDto>> GetCustomersAsync()
        {
            var customers = await _customerRepository.GetListAsync();

            var customerDtos = customers.Select(x => ObjectMapper.Map<Customer, CustomerDto>(x)).ToList();

            return customerDtos;
        }

        public async Task UpdateCustomerAsync(string id, UpdateCustomerDto customerDto)
        {
            var customer = await _customerManager.UpdateAsync(id, customerDto.CompanyName);
            customer.Address = customerDto.Address;
            customer.Phone = customerDto.Phone;
            customer.City = customerDto.City;
            customer.ContactName = customerDto.ContactName;
            customer.Country = customer.Country;
            customer.Region = customer.Region;
            customer.ContactTitle = customerDto.ContactTitle;
            customer.Fax = customerDto.Fax;

            await _customerRepository.UpdateAsync(customer);
        }

        public async Task<IEnumerable<CustomersByCountryDto>> GetCustomersByCountry()
        {
            var stastics = await _customersByCountryRepository.GetListAsync();

            var stasticsDto = ObjectMapper.Map<List<CustomersByCountry>, List<CustomersByCountryDto>>(stastics);

            return stasticsDto;
        }

        public async Task<IEnumerable<PurchasesByCustomerDto>> GetPurchasesByCustomer()
        {
            var stastics = await _purchasesByCustomerRepository.GetListAsync();

            var stasticsDto = ObjectMapper.Map<List<PurchasesByCustomer>, List<PurchasesByCustomerDto>>(stastics);

            return stasticsDto;
        }
    }
}
