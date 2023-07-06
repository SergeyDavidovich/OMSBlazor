using OMSBlazor.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OMSBlazor.Interfaces
{
    public interface IOrderApplicationService
    {
        public Task SaveOrderAsync(CreateOrderDto createOrderDto);
    }
}
