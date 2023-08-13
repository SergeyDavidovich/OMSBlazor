using OMSBlazor.Dto.Order;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.Interfaces
{
    public interface IOrderApplicationService
    {
        public Task<OrderDto> SaveOrderAsync(CreateOrderDto createOrderDto);
    }
}
