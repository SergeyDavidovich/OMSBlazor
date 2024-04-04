using OMSBlazor.Dto.Order;
using OMSBlazor.Application.Contracts.Interfaces;
using OMSBlazor.Northwind.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using OMSBlazor.DomainManagers.Order;
using OMSBlazor.Blazor.Services;
using QuestPDF.Fluent;

namespace OMSBlazor.Application.ApplicationServices
{
    public class OrderApplicationService : ApplicationService, IOrderApplicationService
    {
        private readonly IRepository<Order, int> _orderRepository;
        private readonly IRepository<Employee, int> _employeeRepository;
        private readonly IRepository<Customer, string> _customerRepository;
        private readonly IRepository<OrderDetail> _orderDetailsRepository;
        private readonly IRepository<Product, int> _productRepository;
        private readonly IOrderManager _orderManager;

        public OrderApplicationService(
            IRepository<Order, int> orderRepository,
            IRepository<Employee, int> employeeRepository,
            IRepository<Customer, string> customerRepository,
            IRepository<Product, int> productRepository,
            IRepository<OrderDetail> orderDetailsRepository,
            IOrderManager orderManager)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _productRepository = productRepository;
            _orderDetailsRepository = orderDetailsRepository;
            _orderManager = orderManager;
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
            var orders = (await _orderRepository.WithDetailsAsync(x => x.OrderDetails)).ToList();

            var orderDtos = ObjectMapper.Map<List<Order>, List<OrderDto>>(orders);

            return orderDtos;
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
    }
}
