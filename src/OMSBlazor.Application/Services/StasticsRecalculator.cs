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
        private readonly IRepository<Order, int> _orderRepository;
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
            IRepository<OrderDetail> orderDetailRepository,
            IRepository<Order,int> orderRepository)
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
            _orderRepository = orderRepository;
        }

        public async Task RecalculateStastics(int orderId)
        {
            var customers = await _customerRepostiory.GetListAsync();
            var categories = await _categoryRepository.GetListAsync();
            var products = await _productRepository.GetListAsync();
            var allOrderDetails = await _orderDetailRepository.GetListAsync();
            var employees = await _employeeRepository.GetListAsync();

            var order = await _orderRepository.SingleAsync(x => x.Id == orderId);
            var orderDetails = allOrderDetails.Where(x => x.OrderId == orderId).ToList();

            await RecalculateOrdersByCountries(order, customers);
            await RecalculateSalesByCategories(orderDetails, categories, products);
            await RecalculateSummaries(order, allOrderDetails);
            await RecalculateSalesByCountries(order, orderDetails, customers);
            await RecalculatePurchasesByCustomers(order, orderDetails, customers);
            await RecalculateSalesByEmployee(order, orderDetails);
        }

        private async Task RecalculateSalesByEmployee(Order order, List<OrderDetail> orderDetails)
        {
            var check = orderDetails.Sum(x => x.Quantity * x.UnitPrice);

            var salesByEmployee = await _salesByEmployeeRepository.SingleAsync(x => x.ID == order.EmployeeId);
            salesByEmployee.Sales = salesByEmployee.Sales + (decimal)check;

            await _salesByEmployeeRepository.UpdateAsync(salesByEmployee);
        }

        private async Task RecalculatePurchasesByCustomers(Order order, List<OrderDetail> orderDetails, List<Customer> customers)
        {
            var check = orderDetails.Sum(x => x.Quantity * x.UnitPrice);

            var companyName = customers.Single(x => x.Id == order.CustomerId).CompanyName;
            var purchasesByCustomer = await _purchasesByCustomerRepository.SingleAsync(x => x.CompanyName == companyName);
            purchasesByCustomer.Purchases = purchasesByCustomer.Purchases + (decimal)check;

            await _purchasesByCustomerRepository.UpdateAsync(purchasesByCustomer);
        }

        private async Task RecalculateSalesByCountries(Order order, List<OrderDetail> orderDetails, List<Customer> customers)
        {
            var check = orderDetails.Sum(x => x.Quantity * x.UnitPrice);

            var countryName = customers.Single(x => x.Id == order.CustomerId).Country;
            var salesByCountry = await _salesByCountryRepository.SingleAsync(x => x.CountryName == countryName);
            salesByCountry.Sales = salesByCountry.Sales + (decimal)check;

            await _salesByCountryRepository.UpdateAsync(salesByCountry);
        }

        private async Task RecalculateSummaries(Order order, List<OrderDetail> orderDetails)
        {
            var check = orderDetails.Where(x => x.OrderId == order.Id).Sum(x => x.Quantity * x.UnitPrice);

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

                var summary = await _summaryRepository.SingleAsync(x => x.SummaryName == OMSBlazorStasticsNames.AverageCheck);
                summary.SummaryValue = averageCheck;

                await _summaryRepository.UpdateAsync(summary);
            }

            async Task UpdateOrdersCount()
            {
                var ordersQuantity = await _summaryRepository.SingleAsync(x => x.SummaryName == OMSBlazorStasticsNames.OrdersQuantity);
                ordersQuantity.SummaryValue = ordersQuantity.SummaryValue + 1;

                await _summaryRepository.UpdateAsync(ordersQuantity);
            }

            async Task UpdateOverallSales()
            {
                var overallSales = await _summaryRepository.SingleAsync(x => x.SummaryName == OMSBlazorStasticsNames.OverallSales);
                overallSales.SummaryValue = overallSales.SummaryValue + check;
                await _summaryRepository.UpdateAsync(overallSales);
            }

            async Task UpdateMaxCheck()
            {
                var maxCheck = await _summaryRepository.SingleAsync(x => x.SummaryName == OMSBlazorStasticsNames.MaxCheck);
                if (maxCheck.SummaryValue < check)
                {
                    maxCheck.SummaryValue = check;

                    await _summaryRepository.UpdateAsync(maxCheck);
                }
            }

            async Task UpdateMinCheck()
            {
                var minCheck = await _summaryRepository.SingleAsync(x => x.SummaryName == OMSBlazorStasticsNames.MinCheck);
                if (minCheck.SummaryValue > check)
                {
                    minCheck.SummaryValue = check;

                    await _summaryRepository.UpdateAsync(minCheck);
                }
            }
        }

        private async Task RecalculateSalesByCategories(List<OrderDetail> orderDetails, List<Category> categories, List<Product> products)
        {
            foreach(var orderDetail in orderDetails)
            {
                var product = products.Single(x => x.Id == orderDetail.ProductId);
                var categoryName = categories.Single(x => x.Id == product.CategoryId).CategoryName;

                var sales = orderDetail.Quantity * orderDetail.UnitPrice;

                var salesByCategory = await _salesByCategoryRepository.SingleAsync(x => x.CategoryName == categoryName);
                salesByCategory.Sales = salesByCategory.Sales + (decimal)sales;

                await _salesByCategoryRepository.UpdateAsync(salesByCategory);
            }
        }

        private async Task RecalculateOrdersByCountries(Order order, List<Customer> customers)
        {
            var countryName = customers.Single(x => x.Id == order.CustomerId).Country;
            var ordersByCountry = await _ordersByCountryRepository.SingleAsync(x => x.CountryName == countryName);
            ordersByCountry.OrdersCount = ordersByCountry.OrdersCount + 1;

            await _ordersByCountryRepository.UpdateAsync(ordersByCountry);
        }
    }
}
