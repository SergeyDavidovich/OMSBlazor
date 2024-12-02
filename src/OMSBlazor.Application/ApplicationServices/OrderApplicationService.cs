using OMSBlazor.Dto.Order;
using OMSBlazor.Northwind.OrderAggregate;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using OMSBlazor.DomainManagers.Order;
using OMSBlazor.Blazor.Services;
using QuestPDF.Fluent;
using OMSBlazor.Dto.Order.Stastics;
using OMSBlazor.Northwind.Stastics;
using OMSBlazor.Interfaces.ApplicationServices;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using System;

namespace OMSBlazor.Application.ApplicationServices
{
    public class OrderApplicationService : ApplicationService, IOrderApplicationService
    {
        private readonly IRepository<Order, int> _orderRepository;
        private readonly IReadOnlyRepository<Order, int> _orderReadOnlyRepository; // AsNoTracking behind the scenes for Get requests 
        private readonly IRepository<Employee, int> _employeeRepository;
        private readonly IRepository<Customer, string> _customerRepository;
        private readonly IRepository<OrderDetail> _orderDetailsRepository;
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<OrdersByCountry, int> _ordersByCountryRepository;
        private readonly IRepository<SalesByCategory, int> _salesByCategoryRepository;
        private readonly IRepository<SalesByCountry, int> _salesByCountryRepository;
        private readonly IRepository<Summary, int> _summaryRepository;
        private readonly IOrderManager _orderManager;
        private readonly IDistributedCache<IQueryable<Order>> _orderCache; // added InMemory cache for orders

        public OrderApplicationService(
            IRepository<Order, int> orderRepository,
            IReadOnlyRepository<Order, int> orderReadOnlyRepository,
            IRepository<Employee, int> employeeRepository,
            IRepository<Customer, string> customerRepository,
            IRepository<Product, int> productRepository,
            IRepository<OrderDetail> orderDetailsRepository,
            IOrderManager orderManager,
            IRepository<OrdersByCountry, int> ordersByCountryRepository,
            IRepository<SalesByCategory, int> salesByCategoryRepository,
            IRepository<SalesByCountry, int> salesByCountryRepository,
            IRepository<Summary, int> summaryRepository,
            IDistributedCache<IQueryable<Order>> orderCache
            )
        {
            _orderRepository = orderRepository;
            _orderReadOnlyRepository = orderReadOnlyRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _productRepository = productRepository;
            _orderDetailsRepository = orderDetailsRepository;
            _orderManager = orderManager;
            _ordersByCountryRepository = ordersByCountryRepository;
            _salesByCategoryRepository = salesByCategoryRepository;
            _salesByCountryRepository = salesByCountryRepository;
            _summaryRepository = summaryRepository;
            _orderCache = orderCache;
        }

        public async Task<OrderDto> SaveOrderAsync(CreateOrderDto createOrderDto)
        {
            var order = await _orderManager.CreateAsync(createOrderDto.EmployeeId, createOrderDto.CustomerId);
            foreach (var orderDetailDto in createOrderDto.OrderDetails)
            {
                order.AddOrderDetail(orderDetailDto.ProductId, orderDetailDto.Quantity, orderDetailDto.UnitPrice, orderDetailDto.Discount);
            }

            order.RequiredDate = createOrderDto.RequiredDate;
            order.ShipRegion = createOrderDto.ShipRegion;
            order.ShipName = createOrderDto.ShipName;
            order.ShipCountry = createOrderDto.ShipCountry;
            order.ShipCity = createOrderDto.ShipCity;
            order.ShipAddress = createOrderDto.ShipAddress;
            order.ShipPostalCode = createOrderDto.ShipPostalCode;

            await _orderRepository.InsertAsync(order);

            var orderDto = ObjectMapper.Map<Order, OrderDto>(order);

            return orderDto;
        }

        public async Task<List<OrderDto>> GetOrdersAsync()
        {
            // InMemory cache for orders
            var ordersQuery = await _orderCache.GetOrAddAsync(
                "ordersQuery",
                async () => await _orderReadOnlyRepository.WithDetailsAsync(x => x.OrderDetails),
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = System.DateTimeOffset.Now.AddHours(1)
                }, null, false);

            return ObjectMapper.Map<List<Order>, List<OrderDto>>(ordersQuery.ToList());
        }

        public async Task<OrderDto> GetOrderAsync(int id)
        {
            var order = await _orderRepository.GetAsync(id);

            var orderDto = ObjectMapper.Map<Order, OrderDto>(order);

            return orderDto;
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _orderRepository.DeleteAsync(id);
        }

        public async Task<byte[]> GetInvoiceAsync(int id)
        {
            var order = await _orderRepository.GetAsync(x => x.Id == id);
            var orderDetails = await _orderDetailsRepository.GetListAsync(x => x.OrderId == id);
            var customer = await _customerRepository.GetAsync(order.CustomerId);
            var employee = await _employeeRepository.GetAsync(order.EmployeeId);
            var products = await _productRepository.GetListAsync();

            var pdfInvoiceModel = new InvoiceModel();
            pdfInvoiceModel.InvoiceNumber = id;
            pdfInvoiceModel.OrderDate = order.OrderDate;
            pdfInvoiceModel.EmployeeFullName = $"{employee.FirstName} {employee.LastName}";
            pdfInvoiceModel.CustomerName = $"{customer.CompanyName}";

            foreach (var orderDetail in orderDetails)
            {
                pdfInvoiceModel.Items.Add(new OrderItem()
                {
                    ProductName = products.Single(x => x.Id == orderDetail.ProductId).ProductName,
                    Price = (decimal)orderDetail.UnitPrice,
                    Discount = (int)orderDetail.Discount,
                    Quantity = orderDetail.Quantity
                });
            }
            var document = new InvoiceDocument(pdfInvoiceModel);
            var arr = document.GeneratePdf();
            return arr;
        }

        public async Task<IEnumerable<OrdersByCountryDto>> GetOrdersByCountriesAsync()
        {
            var stastics = await _ordersByCountryRepository.GetListAsync();

            var stasticsDto = ObjectMapper.Map<List<OrdersByCountry>, List<OrdersByCountryDto>>(stastics);

            return stasticsDto;
        }

        public async Task<IEnumerable<SalesByCategoryDto>> GetSalesByCategoriesAsync()
        {
            var stastics = await _salesByCategoryRepository.GetListAsync();

            var stasticsDto = ObjectMapper.Map<List<SalesByCategory>, List<SalesByCategoryDto>>(stastics);

            return stasticsDto;
        }

        public async Task<IEnumerable<SalesByCountryDto>> GetSalesByCountriesAsync()
        {
            var stastics = await _salesByCountryRepository.GetListAsync();

            var stasticsDto = ObjectMapper.Map<List<SalesByCountry>, List<SalesByCountryDto>>(stastics);

            return stasticsDto;
        }

        public async Task<IEnumerable<SummaryDto>> GetSummariesAsync()
        {
            var stastics = await _summaryRepository.GetListAsync();

            var stasticsDto = ObjectMapper.Map<List<Summary>, List<SummaryDto>>(stastics);

            return stasticsDto;
        }

        public async Task<IQueryable<OrderDto>> GetOrdersQueryableAsync()
        {
            var queryable = await _orderRepository.GetQueryableAsync();

            return queryable.Select(x => ObjectMapper.Map<Order, OrderDto>(x));
        }

        public async Task SetPaymentId(int orderId, Guid paymentId)
        {
            var order = await _orderRepository.GetAsync(orderId);
            order.SetPaymentId(paymentId);

            await _orderRepository.UpdateAsync(order);
        }
    }
}