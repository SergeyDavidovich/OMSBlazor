using Microsoft.Extensions.DependencyInjection;
using OMSBlazor.Dto.Category;
using OMSBlazor.Dto.Customer;
using OMSBlazor.Dto.Order;
using OMSBlazor.Dto.Order.Stastics;
using OMSBlazor.Dto.Product;
using OMSBlazor.Interfaces.ApplicationServices;
using OMSBlazor.Interfaces.Services;
using OMSBlazor.Northwind.OrderAggregate;
using OMSBlazor.Northwind.Stastics;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace OMSBlazor.Services
{
    [ExposeServices(typeof(IStasticsRecalculator))]
    public class StasticsRecalculator : IStasticsRecalculator, ITransientDependency
    {
        private readonly IRepository<Product,int> _productRepository;
        private readonly IRepository<Category, int> _categoryRepository;
        private readonly IRepository<Employee, int> _employeeRepository;
        private readonly IRepository<Customer, string> _customerRepostiory;
        private readonly IRepository<CustomersByCountry, string> _customersByCountryRepository;
        private readonly IRepository<OrdersByCountry, string> _ordersByCountryRepository;
        private readonly IRepository<PurchasesByCustomer, string> _purchasesByCustomerRepository;
        private readonly IRepository<SalesByCategory, string> _salesByCategoryRepository;
        private readonly IRepository<SalesByCountry, string> _salesByCountryRepository;
        private readonly IRepository<SalesByEmployee, int> _salesByEmployeeRepository;
        private readonly IRepository<Summary, string> _summaryRepository;
        private readonly IRepository<OrderDetail> _orderDetailRepository;

        public StasticsRecalculator(
            IRepository<CustomersByCountry, string> customersByCountryRepository,
            IRepository<OrdersByCountry, string> ordersByCountryRepository,
            IRepository<PurchasesByCustomer, string> purchasesByCustomerRepository,
            IRepository<SalesByCategory, string> salesByCategoryRepository,
            IRepository<SalesByCountry, string> salesByCountryRepository,
            IRepository<SalesByEmployee, int> salesByEmployeeRepository,
            IRepository<Customer, string> customerRepository,
            IRepository<Employee, int> employeeRepository,
            IRepository<Category, int> categoryRepository,
            IRepository<Summary, string> summaryRepository,
            IRepository<Product, int> productRepository,
            IRepository<OrderDetail> orderDetailRepository)
        {
            _customersByCountryRepository = customersByCountryRepository;
            _ordersByCountryRepository = ordersByCountryRepository;
            _purchasesByCustomerRepository = purchasesByCustomerRepository;
            _salesByCategoryRepository = salesByCategoryRepository;
            _salesByCountryRepository = salesByCountryRepository;
            _salesByEmployeeRepository = salesByEmployeeRepository;
            _customerRepostiory = customerRepository;
            _employeeRepository = employeeRepository;
            _categoryRepository = categoryRepository;
            _summaryRepository = summaryRepository;
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task RecalculateStastics(OrderDto orderDto)
        {
            var customers = await _customerRepostiory.GetListAsync();
            var categories = await _categoryRepository.GetListAsync();
            var products = await _productRepository.GetListAsync();
            var orderDetails = await _orderDetailRepository.GetListAsync();
            var employees = await _employeeRepository.GetListAsync();

            await RecalculateOrdersByCountries(orderDto, customers);
            await RecalculateSalesByCategories(orderDto, categories, products);
            await RecalculateSummaries(orderDto, orderDetails);
            await RecalculateSalesByCountries(orderDto, customers);
            await RecalculatePurchasesByCustomers(orderDto, customers);
            await RecalculateSalesByEmployee(orderDto, employees);
        }

        private async Task RecalculateSalesByEmployee(OrderDto orderDto, object employees)
        {
            var check = orderDto.OrderDetails.Sum(x => x.Quantity * x.UnitPrice);

            var oldSalesByEmployee = await _salesByEmployeeRepository.SingleAsync(x => x.Id == orderDto.EmployeeId);
            var newSalesByEmployee = new SalesByEmployee(oldSalesByEmployee.Id, oldSalesByEmployee.LastName);
            newSalesByEmployee.Sales = oldSalesByEmployee.Sales + (decimal)check;

            await _salesByEmployeeRepository.UpdateAsync(newSalesByEmployee);
        }

        private async Task RecalculatePurchasesByCustomers(OrderDto orderDto, List<Customer> customers)
        {
            var check = orderDto.OrderDetails.Sum(x => x.Quantity * x.UnitPrice);

            var companyName = customers.Single(x => x.Id == orderDto.CustomerId).CompanyName;
            var oldPurchasesByCustomer = await _purchasesByCustomerRepository.SingleAsync(x => x.CompanyName == companyName);
            var newPurchasesByCustomer = new PurchasesByCustomer(companyName);
            newPurchasesByCustomer.Purchases = oldPurchasesByCustomer.Purchases + (decimal)check;

            await _purchasesByCustomerRepository.UpdateAsync(newPurchasesByCustomer);
        }

        private async Task RecalculateSalesByCountries(OrderDto orderDto, List<Customer> customers)
        {
            var check = orderDto.OrderDetails.Sum(x => x.Quantity * x.UnitPrice);

            var countryName = customers.Single(x => x.Id == orderDto.CustomerId).Country;
            var oldSalesByCountry = await _salesByCountryRepository.SingleAsync(x => x.CountryName == countryName);
            var newSalesByCountry = new SalesByCountry(countryName);
            newSalesByCountry.Sales = oldSalesByCountry.Sales + (decimal)check;

            await _salesByCountryRepository.UpdateAsync(newSalesByCountry);
        }

        private async Task RecalculateSummaries(OrderDto orderDto, List<OrderDetail> orderDetails)
        {
            var check = orderDto.OrderDetails.Sum(x => x.Quantity * x.UnitPrice);

            await UpdateOverallSales();
            await UpdateMaxCheck();
            await UpdateMinCheck();
            await UpdateOrdersCount();
            await UpdateAverageCheck();

            async Task UpdateAverageCheck()
            {
                var averageCheck = await Task.Run(
                    () => orderDetails
                    .GroupBy(x => x.OrderId)
                    .Select(x => x.Sum(y => y.Discount * y.UnitPrice))
                    .Average());

                await _summaryRepository.UpdateAsync(new Summary()
                {
                    SummaryName = OMSBlazorStasticsNames.AverageCheck,
                    SummaryValue = averageCheck
                });
            }

            async Task UpdateOrdersCount()
            {
                var oldOrdersQuantity = await _summaryRepository.SingleAsync(x => x.SummaryName == OMSBlazorStasticsNames.OrdersQuantity);

                await _summaryRepository.UpdateAsync(new Summary()
                {
                    SummaryName = OMSBlazorStasticsNames.OrdersQuantity,
                    SummaryValue = oldOrdersQuantity.SummaryValue + 1
                });
            }

            async Task UpdateOverallSales()
            {
                var oldOverallSales = await _summaryRepository.SingleAsync(x => x.SummaryName == OMSBlazorStasticsNames.OverallSales);
                var newOverallSales = new Summary();
                newOverallSales.SummaryName = OMSBlazorStasticsNames.OverallSales;
                newOverallSales.SummaryValue = oldOverallSales.SummaryValue + check;
                await _summaryRepository.UpdateAsync(newOverallSales);
            }

            async Task UpdateMaxCheck()
            {
                var maxCheck = await _summaryRepository.SingleAsync(x => x.SummaryName == OMSBlazorStasticsNames.MaxCheck);
                if (maxCheck.SummaryValue < check)
                {
                    var newMaxCheck = new Summary();
                    newMaxCheck.SummaryName = OMSBlazorStasticsNames.MaxCheck;
                    newMaxCheck.SummaryValue = check;

                    await _summaryRepository.UpdateAsync(newMaxCheck);
                }
            }

            async Task UpdateMinCheck()
            {
                var minCheck = await _summaryRepository.SingleAsync(x => x.SummaryName == OMSBlazorStasticsNames.MinCheck);
                if (minCheck.SummaryValue > check)
                {
                    var newMinCheck = new Summary();
                    newMinCheck.SummaryName = OMSBlazorStasticsNames.MinCheck;
                    newMinCheck.SummaryValue = check;

                    await _summaryRepository.UpdateAsync(newMinCheck);
                }
            }
        }

        private async Task RecalculateSalesByCategories(OrderDto orderDto, List<Category> categories, List<Product> products)
        {
            foreach(var orderDetail in orderDto.OrderDetails)
            {
                var product = products.Single(x => x.Id == orderDetail.ProductId);
                var categoryName = categories.Single(x => x.Id == product.CategoryId).CategoryName;

                var sales = orderDetail.Quantity * orderDetail.UnitPrice;

                var oldSalesByCategory = await _salesByCategoryRepository.SingleAsync(x => x.CategoryName == categoryName);

                var newSalesByCategory = new SalesByCategory(categoryName);
                newSalesByCategory.Sales = oldSalesByCategory.Sales + (decimal)sales;

                await _salesByCategoryRepository.UpdateAsync(newSalesByCategory);
            }
        }

        private async Task RecalculateOrdersByCountries(OrderDto order, List<Customer> customers)
        {
            var countryName = customers.Single(x => x.Id == order.CustomerId).Country;
            var oldOrdersByCountry = await _ordersByCountryRepository.SingleAsync(x => x.CountryName == countryName);
            var newOrdersByCountry = new OrdersByCountry(oldOrdersByCountry.CountryName);
            newOrdersByCountry.OrdersCount = oldOrdersByCountry.OrdersCount + 1;

            await _ordersByCountryRepository.UpdateAsync(newOrdersByCountry);
        }
    }
}
