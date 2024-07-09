using DynamicData.Binding;
using DynamicData;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Text.Json;
using OMSBlazor.Client.Constants;
using OMSBlazor.Dto.Order.Stastics;

namespace OMSBlazor.Client.Pages.Dashboard.OrderStastics
{
    public class OrderStasticsViewModel : ReactiveObject
    {
        private readonly ReadOnlyObservableCollection<OrdersByCountryDto> _ordersByCountries;
        private readonly ReadOnlyObservableCollection<SalesByCategoryDto> _salesByCategories;
        private readonly ReadOnlyObservableCollection<SalesByCountryDto> _salesByCountries;
        private readonly ReadOnlyObservableCollection<SummaryDto> _summaries;

        private readonly SourceCache<OrdersByCountryDto, string> _ordersByCountriesSource;
        private readonly SourceCache<SalesByCategoryDto, string> _salesByCategoriesSource;
        private readonly SourceCache<SalesByCountryDto, string> _salesByCountriesSource;
        private readonly SourceCache<SummaryDto, string> _summariesSource;

        public OrderStasticsViewModel()
        {
            _ordersByCountriesSource = new(x => x.CountryName);
            _salesByCategoriesSource = new(x => x.CategoryName);
            _salesByCountriesSource = new(x => x.CountryName);
            _summariesSource = new(x => x.SummaryName);

            _ordersByCountriesSource.Connect()
                .Sort(SortExpressionComparer<OrdersByCountryDto>.Descending(x => x.OrdersCount))
                .Top(10)
                .Bind(out _ordersByCountries)
                .Subscribe();

            _salesByCategoriesSource.Connect()
                .Bind(out _salesByCategories)
                .Subscribe();

            _salesByCountriesSource.Connect()
                .Sort(SortExpressionComparer<SalesByCountryDto>.Descending(x => x.Sales))
                .Bind(out _salesByCountries)
                .Subscribe();

            _summariesSource.Connect()
                .Bind(out _summaries)
                .Subscribe();
        }

        public async Task OnNavigatedTo()
        {
            if (HttpClient is null)
            {
                throw new NullReferenceException(nameof(OrderStasticsViewModel.HttpClient));
            }

            var ordersByCountriesJson = await HttpClient.GetStringAsync(BackEndEnpointURLs.OrderEndpoints.OrdersByCountries);
            var salesByCategoriesJson = await HttpClient.GetStringAsync(BackEndEnpointURLs.OrderEndpoints.SalesByCategories);
            var salesByCountriesJson = await HttpClient.GetStringAsync(BackEndEnpointURLs.OrderEndpoints.SalesByCountries);
            var summariesJson = await HttpClient.GetStringAsync(BackEndEnpointURLs.OrderEndpoints.Summaries);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var ordersByCountries = JsonSerializer.Deserialize<List<OrdersByCountryDto>>(ordersByCountriesJson, options);
            var salesByCategories = JsonSerializer.Deserialize<List<SalesByCategoryDto>>(salesByCategoriesJson, options);
            var salesByCountries = JsonSerializer.Deserialize<List<SalesByCountryDto>>(salesByCountriesJson, options);
            var summaries = JsonSerializer.Deserialize<List<SummaryDto>>(summariesJson, options);

            _ordersByCountriesSource.AddOrUpdate(ordersByCountries);
            _salesByCategoriesSource.AddOrUpdate(salesByCategories);
            _salesByCountriesSource.AddOrUpdate(salesByCountries);
            _summariesSource.AddOrUpdate(summaries);
        }

        public async Task UpdateStastics()
        {
            if (HttpClient is null)
            {
                throw new NullReferenceException(nameof(OrderStasticsViewModel.HttpClient));
            }

            var newOrdersByCountriesJson = await HttpClient.GetStringAsync(BackEndEnpointURLs.OrderEndpoints.OrdersByCountries);
            var newSalesByCategoriesJson = await HttpClient.GetStringAsync(BackEndEnpointURLs.OrderEndpoints.SalesByCategories);
            var newSalesByCountriesJson = await HttpClient.GetStringAsync(BackEndEnpointURLs.OrderEndpoints.SalesByCountries);
            var newSummariesJson = await HttpClient.GetStringAsync(BackEndEnpointURLs.OrderEndpoints.Summaries);

            var newOrdersByCountries = JsonSerializer.Deserialize<List<OrdersByCountryDto>>(newOrdersByCountriesJson);
            var newSalesByCategories = JsonSerializer.Deserialize<List<SalesByCategoryDto>>(newSalesByCategoriesJson);
            var newSalesByCountries = JsonSerializer.Deserialize<List<SalesByCountryDto>>(newSalesByCountriesJson);
            var newSummaries = JsonSerializer.Deserialize<List<SummaryDto>>(newSummariesJson);

            foreach (var summary in _summariesSource.Items.ToList())
            {
                summary.SummaryValue = newSummaries.Single(x => x.SummaryName == summary.SummaryName).SummaryValue;
                _summariesSource.Refresh(summary);
            }

            foreach (var salesByCountry in _salesByCountriesSource.Items.ToList())
            {
                salesByCountry.Sales = newSalesByCountries.Single(x => x.CountryName == salesByCountry.CountryName).Sales;
                _salesByCountriesSource.Refresh(salesByCountry);
            }

            foreach (var ordersByCountry in _ordersByCountriesSource.Items.ToList())
            {
                ordersByCountry.OrdersCount = newOrdersByCountries.Single(x => x.CountryName == ordersByCountry.CountryName).OrdersCount;
                _ordersByCountriesSource.Refresh(ordersByCountry);
            }

            foreach (var salesByCategory in _salesByCategoriesSource.Items.ToList())
            {
                salesByCategory.Sales = newSalesByCategories.Single(x => x.CategoryName == salesByCategory.CategoryName).Sales;
                _salesByCategoriesSource.Refresh(salesByCategory);
            }
        }

        public ReadOnlyObservableCollection<OrdersByCountryDto> OrdersByCountries => _ordersByCountries;

        public ReadOnlyObservableCollection<SalesByCategoryDto> SalesByCategories => _salesByCategories;

        public ReadOnlyObservableCollection<SalesByCountryDto> SalesByCountries => _salesByCountries;

        public ReadOnlyObservableCollection<SummaryDto> Summaries => _summaries;

        public HttpClient? HttpClient { get; set; }
    }
}
