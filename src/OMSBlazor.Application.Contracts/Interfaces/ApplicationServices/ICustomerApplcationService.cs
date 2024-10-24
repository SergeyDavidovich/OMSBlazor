﻿using OMSBlazor.Dto.Customer;
using OMSBlazor.Dto.Customer.Stastics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OMSBlazor.Interfaces.ApplicationServices
{
    public interface ICustomerApplcationService : IApplicationService
    {
        public Task<List<CustomerDto>> GetCustomersAsync();

        public Task<CustomerDto> GetCustomerAsync(string customerId);

        public Task DeleteCustomerAsync(string customerId);

        public Task CreateCustomerAsync(CreateCustomerDto customerDto);

        public Task UpdateCustomerAsync(string id, UpdateCustomerDto customerDto);

        public Task<IEnumerable<CustomersByCountryDto>> GetCustomersByCountry();

        public Task<IEnumerable<PurchasesByCustomerDto>> GetPurchasesByCustomer();
    }
}
