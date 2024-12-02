using OMSBlazor.Northwind.OrderAggregate;
using OMSBlazor.Northwind.OrderAggregate.Exceptions;
using StripeModule.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace OMSBlazor.DomainManagers.Order
{
    public class OrderManager : DomainService, IOrderManager
    {
        private readonly IRepository<Northwind.OrderAggregate.Order, int> _orderRepository;
        private readonly IGuidGenerator _guidGenerator;

        public OrderManager(IRepository<Northwind.OrderAggregate.Order, int> orderRepository, IGuidGenerator guidGenerator)
        {
            _orderRepository = orderRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task<Northwind.OrderAggregate.Order> CreateAsync(int employeeId, string customerId)
        {
            var lastOrder = (await _orderRepository.GetListAsync()).LastOrDefault();

            var lastOrderId = lastOrder is null ? 0 : lastOrder.Id;

            var order = new Northwind.OrderAggregate.Order(lastOrderId + 1, employeeId, customerId);

            return order;
        }
    }
}
