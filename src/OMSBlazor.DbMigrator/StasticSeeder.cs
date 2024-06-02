using Microsoft.EntityFrameworkCore;
using OMSBlazor.Northwind.OrderAggregate;
using OMSBlazor.Northwind.Stastics;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;

namespace OMSBlazor.DbMigrator
{
    public class StasticSeeder : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Order, int> _ordersRepository;
        private readonly IRepository<Customer, string> _customersRepository;
        private readonly IRepository<Category, int> _categoriesRepository;
        private readonly IRepository<Product, int> _productsRepository;
        private readonly IRepository<Employee, int> _employeeRepository;
        private readonly IRepository<OrderDetail> _orderDetailsRepository;

        private readonly IRepository<OrdersByCountry, int> _ordersByCountriesRepository;
        private readonly IRepository<SalesByCategory, int> _salesByCategoriesRepository;
        private readonly IRepository<SalesByCountry, int> _salesByCountriesRepository;
        private readonly IRepository<CustomersByCountry, int> _customersByCountriesRepository;
        private readonly IRepository<PurchasesByCustomer, int> _purchasesByCustomersRepository;
        private readonly IRepository<SalesByEmployee, int> _salesByEmployeesRepository;
        private readonly IRepository<ProductsByCategory, int> _productsByCatgoriesRepository;
        private readonly IRepository<Summary, int> _summariesRepository;

        private readonly ICurrentTenant _currentTenant;

        public StasticSeeder(
            IRepository<Order, int> ordersRepository,
            IRepository<OrdersByCountry, int> ordersByCountryRepository,
            IRepository<Customer, string> customersRepository,
            IRepository<OrderDetail> orderDetailsRepository,
            IRepository<SalesByCategory, int> salesByCategoriesRepository,
            IRepository<Category, int> categoriesRepository,
            IRepository<Product, int> productsRepository,
            IRepository<Summary, int> summariesRepository,
            IRepository<SalesByCountry, int> salesByCountriesRepository,
            IRepository<CustomersByCountry, int> customersByCountriesRepository,
            IRepository<PurchasesByCustomer, int> purchasesByCustomersRepository,
            IRepository<Employee, int> employeeRepository,
            IRepository<SalesByEmployee, int> salesByEmployeesRepository,
            IRepository<ProductsByCategory, int> productsByCatgoriesRepository,
            ICurrentTenant currentTenant)
        {
            _ordersRepository = ordersRepository;
            _ordersByCountriesRepository = ordersByCountryRepository;
            _customersRepository = customersRepository;
            _orderDetailsRepository = orderDetailsRepository;
            _salesByCategoriesRepository = salesByCategoriesRepository;
            _categoriesRepository = categoriesRepository;
            _productsRepository = productsRepository;
            _summariesRepository = summariesRepository;
            _salesByCountriesRepository = salesByCountriesRepository;
            _customersByCountriesRepository = customersByCountriesRepository;
            _purchasesByCustomersRepository = purchasesByCustomersRepository;
            _employeeRepository = employeeRepository;
            _salesByEmployeesRepository = salesByEmployeesRepository;
            _productsByCatgoriesRepository = productsByCatgoriesRepository;
            _currentTenant = currentTenant;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context?.TenantId))
            {
                var ordersQueryable = await _ordersRepository.GetQueryableAsync();
                var orderDetailsQueryable = await _orderDetailsRepository.GetQueryableAsync();
                var customersQueryable = await _customersRepository.GetQueryableAsync();
                var categoriesQueryable = await _categoriesRepository.GetQueryableAsync();
                var productsQueryable = await _productsRepository.GetQueryableAsync();
                var employeesQueryable = await _employeeRepository.GetQueryableAsync();

                await SeedOrdersByCountryAsync(ordersQueryable, customersQueryable);
                await SeedSalesByCategoriesAsync(orderDetailsQueryable, categoriesQueryable, productsQueryable);
                await SeedStasticsAsync(orderDetailsQueryable, ordersQueryable);
                await SeedSalesByCountriesAsync(orderDetailsQueryable, customersQueryable, ordersQueryable);
                await SeedCustomersByCountriesAsync(customersQueryable);
                await SeedPurchasesByCustomersAsync(orderDetailsQueryable, customersQueryable, ordersQueryable);
                await SeedSalesByEmployeesAsync(orderDetailsQueryable, employeesQueryable, ordersQueryable);
                await SeedProductsByCategoriesAsync(categoriesQueryable, productsQueryable);
            }
        }

        private async Task SeedProductsByCategoriesAsync(IQueryable<Category> categoriesQueryable, IQueryable<Product> productsQueryable)
        {
            if (!await _productsByCatgoriesRepository.AnyAsync())
            {
                var productsByCategories = await categoriesQueryable
                    .Select(category => new ProductsByCategory(category.CategoryName)
                    {
                        Value = productsQueryable.Where(x => x.CategoryId == category.Id).Count()
                    })
                    .ToListAsync();

                await _productsByCatgoriesRepository.InsertManyAsync(productsByCategories);
            }
        }

        private async Task SeedSalesByEmployeesAsync(IQueryable<OrderDetail> orderDetailsQueryable, IQueryable<Employee> employeesQueryable, IQueryable<Order> ordersQueryable)
        {
            if (!await _salesByEmployeesRepository.AnyAsync())
            {
                var groupedOrderDetails = orderDetailsQueryable
                    .GroupBy(orderDetail => new
                    {
                        Id = employeesQueryable.Single(x => x.Id == (ordersQueryable.Single(y => y.Id == orderDetail.OrderId).EmployeeId)).Id,
                        LastName = employeesQueryable.Single(x => x.Id == (ordersQueryable.Single(y => y.Id == orderDetail.OrderId).EmployeeId)).LastName
                    });

                var salesByEmployees = await groupedOrderDetails
                    .Select(orderDetailGroup => new SalesByEmployee(orderDetailGroup.Key.Id, orderDetailGroup.Key.LastName)
                    {
                        Value = (decimal)orderDetailGroup.Sum(orderDetail => orderDetail.Quantity * orderDetail.UnitPrice)
                    })
                    .ToListAsync();

                await _salesByEmployeesRepository.InsertManyAsync(salesByEmployees);
            }
        }

        private async Task SeedPurchasesByCustomersAsync(IQueryable<OrderDetail> orderDetailsQueryable, IQueryable<Customer> customersQueryable, IQueryable<Order> ordersQueryable)
        {
            if (!await _purchasesByCustomersRepository.AnyAsync())
            {
                var groupedOrderDetails = orderDetailsQueryable
                    .GroupBy(orderDetail => customersQueryable.Single(x => x.Id == (ordersQueryable.Single(y => y.Id == orderDetail.OrderId).CustomerId)).CompanyName);

                var purchasesByCustomers = await groupedOrderDetails
                    .Select(orderDetailGroup => new PurchasesByCustomer(orderDetailGroup.Key)
                    {
                        Value = (decimal)orderDetailGroup.Sum(orderDetail => orderDetail.Quantity * orderDetail.UnitPrice)
                    })
                    .ToListAsync();

                await _purchasesByCustomersRepository.InsertManyAsync(purchasesByCustomers);
            }
        }

        private async Task SeedCustomersByCountriesAsync(IQueryable<Customer> customersQueryable)
        {
            if (!await _customersByCountriesRepository.AnyAsync())
            {
                var groupedCustomers = customersQueryable
                    .GroupBy(customer => customer.Country);

                var customersByCountries = await groupedCustomers
                    .Select(customerGroup => new CustomersByCountry(customerGroup.Key)
                    {
                        Value = customerGroup.Count()
                    })
                    .ToListAsync();

                await _customersByCountriesRepository.InsertManyAsync(customersByCountries);
            }
        }

        private async Task SeedSalesByCountriesAsync(IQueryable<OrderDetail> orderDetailsQueryable, IQueryable<Customer> customersQueryable, IQueryable<Order> ordersQueryable)
        {
            if (!await _salesByCountriesRepository.AnyAsync())
            {
                var groupedOrderDetails = orderDetailsQueryable
                    .GroupBy(orderDetail => customersQueryable.Single(x => x.Id == (ordersQueryable.Single(y => y.Id == orderDetail.OrderId).CustomerId)).Country);

                var salesByCountries = await groupedOrderDetails
                    .Select(orderDetailGroup => new SalesByCountry(orderDetailGroup.Key)
                    {
                        Value = (decimal)orderDetailGroup.Sum(orderDetail => orderDetail.Quantity * orderDetail.UnitPrice)
                    })
                    .ToListAsync();

                await _salesByCountriesRepository.InsertManyAsync(salesByCountries);
            }
        }

        private async Task SeedStasticsAsync(IQueryable<OrderDetail> orderDetailsQueryable, IQueryable<Order> ordersQueryable)
        {
            if (!await _summariesRepository.AnyAsync())
            {
                var overallSales = await orderDetailsQueryable.SumAsync(a => a.Quantity * a.UnitPrice);
                var ordersQuantity = await ordersQueryable.CountAsync();
                var groupedOrderDetails = orderDetailsQueryable.GroupBy(od => od.OrderId);
                var ordersChecks = await groupedOrderDetails.Select(god => new { Sales = god.Sum(a => a.Quantity * a.UnitPrice) }).ToListAsync();
                var maxCheck = ordersChecks.Max(a => a.Sales);
                var averageCheck = ordersChecks.Average(a => a.Sales);
                var minCheck = ordersChecks.Min(a => a.Sales);

                await _summariesRepository.InsertAsync(new Summary()
                {
                    Key = OMSBlazorConstants.OverallSales,
                    Value = overallSales
                });
                await _summariesRepository.InsertAsync(new Summary()
                {
                    Key = OMSBlazorConstants.OrdersQuantity,
                    Value = ordersQuantity
                });
                await _summariesRepository.InsertAsync(new Summary()
                {
                    Key = OMSBlazorConstants.MaxCheck,
                    Value = maxCheck
                });
                await _summariesRepository.InsertAsync(new Summary()
                {
                    Key = OMSBlazorConstants.AverageCheck,
                    Value = averageCheck
                });
                await _summariesRepository.InsertAsync(new Summary()
                {
                    Key = OMSBlazorConstants.MinCheck,
                    Value = minCheck
                });
            }
        }

        private async Task SeedSalesByCategoriesAsync(
            IQueryable<OrderDetail> orderDetailsQueryable,
            IQueryable<Category> categoriesQueryable,
            IQueryable<Product> productsQueryable)
        {
            if (!await _salesByCategoriesRepository.AnyAsync())
            {
                var groupedOrderDetail = orderDetailsQueryable
                    .GroupBy(orderDetail => categoriesQueryable.Single(y => y.Id == productsQueryable.Single(x => x.Id == orderDetail.ProductId).CategoryId).CategoryName);

                var salesByCategories = await groupedOrderDetail
                    .Select(orderDetailGroup => new SalesByCategory(orderDetailGroup.Key)
                    {
                        Value = (decimal)orderDetailGroup.Sum(orderDetail => orderDetail.Quantity * orderDetail.UnitPrice)
                    })
                    .ToListAsync();

                await _salesByCategoriesRepository.InsertManyAsync(salesByCategories);
            }
        }

        private async Task SeedOrdersByCountryAsync(IQueryable<Order> ordersQueryable, IQueryable<Customer> customersQueryable)
        {
            if (!(await _ordersByCountriesRepository.AnyAsync()))
            {
                var groupedOrders = ordersQueryable
                    .GroupBy(order => customersQueryable.Single(x => x.Id == order.CustomerId).Country);

                var ordersByCountries = await groupedOrders
                    .Select(orderGroup => new OrdersByCountry(orderGroup.Key)
                    {
                        Value = orderGroup.Count()
                    })
                    .ToListAsync();

                await _ordersByCountriesRepository.InsertManyAsync(ordersByCountries);
            }
        }
    }
}
