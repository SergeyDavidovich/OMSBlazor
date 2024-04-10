using DynamicData;
using OMSBlazor.Application.Contracts.Interfaces;
using OMSBlazor.Dto.Order.Stastics;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace OMSBlazor.Blazor.Pages.Dashboard.OrderStastics
{
    public class OrderStasticsViewModel : ReactiveObject
    {
        private readonly IOrderApplicationService _orderApplicationService;

        private readonly ReadOnlyObservableCollection<OrdersByCountryDto> _ordersByCountries;
        private readonly ReadOnlyObservableCollection<SalesByCategoryDto> _salesByCategories;
        private readonly ReadOnlyObservableCollection<SalesByCountryDto> _salesByCountries;
        private readonly ReadOnlyObservableCollection<SummaryDto> _summaries;

        private readonly SourceCache<OrdersByCountryDto, string> _ordersByCountriesSource;
        private readonly SourceCache<SalesByCategoryDto, string> _salesByCategoriesSource;
        private readonly SourceCache<SalesByCountryDto, string> _salesByCountriesSource;
        private readonly SourceCache<SummaryDto, string> _summariesSource;

        public OrderStasticsViewModel(IOrderApplicationService orderApplicationService)
        {
            _orderApplicationService = orderApplicationService;

            _ordersByCountriesSource = new(x => x.CountryName);
            _salesByCategoriesSource = new(x => x.CategoryName);
            _salesByCountriesSource = new(x => x.CountryName);
            _summariesSource = new(x => x.SummaryName);

            _ordersByCountriesSource.Connect()
                .Bind(out _ordersByCountries)
                .Subscribe();

            _salesByCategoriesSource.Connect()
                .Bind(out _salesByCategories)
                .Subscribe();

            _salesByCountriesSource.Connect()
                .Bind(out _salesByCountries)
                .Subscribe();

            _summariesSource.Connect()
                .Bind(out _summaries)
                .Subscribe();
        }

        public async Task OnNavigatedTo()
        {
            var ordersByCountries = (await _orderApplicationService.GetOrdersByCountriesAsync()).OrderByDescending(x => x.OrdersCount).Take(10);
            var salesByCategories = await _orderApplicationService.GetSalesByCategoriesAsync();
            var salesByCountries = await _orderApplicationService.GetSalesByCountriesAsync();
            var summaries = await _orderApplicationService.GetSummariesAsync();

            _ordersByCountriesSource.AddOrUpdate(ordersByCountries);
            _salesByCategoriesSource.AddOrUpdate(salesByCategories);
            _salesByCountriesSource.AddOrUpdate(salesByCountries);
            _summariesSource.AddOrUpdate(summaries);


        }

        public ReadOnlyObservableCollection<OrdersByCountryDto> OrdersByCountries => _ordersByCountries;

        public ReadOnlyObservableCollection<SalesByCategoryDto> SalesByCategories => _salesByCategories;

        public ReadOnlyObservableCollection<SalesByCountryDto> SalesByCountries => _salesByCountries;

        public ReadOnlyObservableCollection<SummaryDto> Summaries => _summaries;
    }
}
