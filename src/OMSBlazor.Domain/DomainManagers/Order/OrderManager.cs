﻿using OMSBlazor.Northwind.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace OMSBlazor.DomainManagers.Order
{
    public class OrderManager : DomainService, IOrderManager
    {
        private readonly IRepository<Northwind.OrderAggregate.Order, int> _orderRepository;

        public OrderManager(IRepository<Northwind.OrderAggregate.Order, int> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Northwind.OrderAggregate.Order> CreateAsync(int employeeId, string customerId, List<OrderDetail> orderDetails)
        {
            var lastOrderId = (await _orderRepository.GetListAsync()).Last().Id;

            var order = new Northwind.OrderAggregate.Order(lastOrderId + 1, employeeId, customerId);

            foreach (var orderDetail in orderDetails)
            {
                order.AddOrderDetail(orderDetail.ProductId, orderDetail.Quantity, orderDetail.UnitPrice, orderDetail.Discount);
            }

            return order;
        }
    }
}
