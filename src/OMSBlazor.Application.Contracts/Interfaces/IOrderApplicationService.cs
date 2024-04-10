using OMSBlazor.Dto.Order;
using OMSBlazor.Dto.Order.Stastics;
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

        public Task DeleteOrderAsync(int id);

        public Task<byte[]> GetInvoiceAsync(int id);

        public Task<IEnumerable<OrdersByCountryDto>> GetOrdersByCountriesAsync();

        public Task<IEnumerable<SalesByCategoryDto>> GetSalesByCategoriesAsync();

        public Task<IEnumerable<SalesByCountryDto>> GetSalesByCountriesAsync();

        public Task<IEnumerable<SummaryDto>> GetSummariesAsync();
    }
}
