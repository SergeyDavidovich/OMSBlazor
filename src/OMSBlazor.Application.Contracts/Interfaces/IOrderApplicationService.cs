using OMSBlazor.Dto.Order;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OMSBlazor.Application.Contracts.Interfaces
{
    public interface IOrderApplicationService : IApplicationService
    {
        public Task<OrderDto> SaveOrderAsync(CreateOrderDto createOrderDto);

        public Task<List<OrderDto>> GetOrdersAsync();

        public Task<OrderDto> GetOrderAsync(int id);
    }
}
