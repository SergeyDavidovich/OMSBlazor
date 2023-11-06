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

namespace OMSBlazor.Application.ApplicationServices
{
    public class OrderApplicationService : ApplicationService, IOrderApplicationService
    {
        private readonly IRepository<Order, int> _orderRepository;
        private readonly IOrderManager _orderManager;

        public OrderApplicationService(IRepository<Order, int> orderRepository, IOrderManager orderManager)
        {
            _orderRepository = orderRepository;
            _orderManager = orderManager;
        }

        public async Task<OrderDto> SaveOrderAsync(CreateOrderDto createOrderDto)
        {
            var orderDetails = ObjectMapper.Map<List<OrderDetailDto>, List<OrderDetail>>(createOrderDto.OrderDetails);
            var order = await _orderManager.CreateAsync(createOrderDto.EmployeeId, createOrderDto.CustomerId, orderDetails);

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
    }
}
