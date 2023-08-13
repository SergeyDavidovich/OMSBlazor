using OMSBlazor.Dto.Order;
using OMSBlazor.Interfaces;
using OMSBlazor.Northwind.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OMSBlazor.ApplicationServices
{
    public class OrderApplicationService : ApplicationService, IOrderApplicationService
    {
        private readonly IRepository<Order, int> _orderRepository;

        public OrderApplicationService(IRepository<Order, int> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDto> SaveOrderAsync(CreateOrderDto createOrderDto)
        {
            var lastOrderId = (await _orderRepository.GetListAsync()).Last().Id;

            var order = new Order(lastOrderId + 1, createOrderDto.EmployeeId, createOrderDto.CustomerId);

            foreach (var orderDetail in createOrderDto.OrderDetails)
            {
                order.AddOrderDetail(orderDetail.ProductId, orderDetail.Quantity, orderDetail.UnitPrice, orderDetail.Discount);
            }

            await _orderRepository.InsertAsync(order);

            var orderDto = ObjectMapper.Map<Order, OrderDto>(order);

            return orderDto;
        }
    }
}
