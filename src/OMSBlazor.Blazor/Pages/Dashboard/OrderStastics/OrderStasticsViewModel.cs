﻿using DynamicData;
using DynamicData.Binding;
using OMSBlazor.Dto.Customer.Stastics;
using OMSBlazor.Dto.Order.Stastics;
using OMSBlazor.Interfaces.ApplicationServices;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using static OMSBlazor.Blazor.Pages.Order.Create.CreateViewModel;

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
            var ordersByCountries = await _orderApplicationService.GetOrdersByCountriesAsync();
            var salesByCategories = await _orderApplicationService.GetSalesByCategoriesAsync();
            var salesByCountries = await _orderApplicationService.GetSalesByCountriesAsync();
            var summaries = await _orderApplicationService.GetSummariesAsync();

            _ordersByCountriesSource.AddOrUpdate(ordersByCountries);
            _salesByCategoriesSource.AddOrUpdate(salesByCategories);
            _salesByCountriesSource.AddOrUpdate(salesByCountries);
            _summariesSource.AddOrUpdate(summaries);
        }

        public async Task UpdateStastics()
        {
            var newOrdersByCountries = await _orderApplicationService.GetOrdersByCountriesAsync();
            var newSalesByCategories = await _orderApplicationService.GetSalesByCategoriesAsync();
            var newSummaries = await _orderApplicationService.GetSummariesAsync();
            var newSalesByCountries = await _orderApplicationService.GetSalesByCountriesAsync();

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

            foreach(var ordersByCountry in _ordersByCountriesSource.Items.ToList())
            {
                ordersByCountry.OrdersCount = newOrdersByCountries.Single(x => x.CountryName == ordersByCountry.CountryName).OrdersCount;
                _ordersByCountriesSource.Refresh(ordersByCountry);
            }

            foreach(var salesByCategory in _salesByCategoriesSource.Items.ToList())
            {
                salesByCategory.Sales = newSalesByCategories.Single(x => x.CategoryName == salesByCategory.CategoryName).Sales;
                _salesByCategoriesSource.Refresh(salesByCategory);
            }
        }

        public ReadOnlyObservableCollection<OrdersByCountryDto> OrdersByCountries => _ordersByCountries;

        public ReadOnlyObservableCollection<SalesByCategoryDto> SalesByCategories => _salesByCategories;

        public ReadOnlyObservableCollection<SalesByCountryDto> SalesByCountries => _salesByCountries;

        public ReadOnlyObservableCollection<SummaryDto> Summaries => _summaries;
    }
}