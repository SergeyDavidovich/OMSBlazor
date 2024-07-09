﻿using DynamicData.Binding;
using DynamicData;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Text.Json;
using OMSBlazor.Client.Constants;
using OMSBlazor.Dto.Employee.Stastics;

namespace OMSBlazor.Client.Pages.Dashboard.EmployeeStastics
{
    public class EmployeeStasticsViewModel : ReactiveObject
    {
        private readonly ReadOnlyObservableCollection<SalesByEmployeeDto> _salesByEmployee;

        private readonly SourceCache<SalesByEmployeeDto, int> _salesByEmployeeSource;

        public EmployeeStasticsViewModel()
        {
            _salesByEmployeeSource = new(x => x.ID);

            _salesByEmployeeSource
                .Connect()
                .Sort(SortExpressionComparer<SalesByEmployeeDto>.Descending(x => x.Sales))
                .Bind(out _salesByEmployee)
                .Subscribe();
        }

        public async Task OnNavigatedTo()
        {
            if (HttpClient is null)
            {
                throw new NullReferenceException(nameof(EmployeeStasticsViewModel.HttpClient));
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var salesByEmployeesJson = await HttpClient.GetStringAsync(BackEndEnpointURLs.EmployeeEndpoints.SalesByEmployees);
            var salesByEmployees = JsonSerializer.Deserialize<List<SalesByEmployeeDto>>(salesByEmployeesJson, options);

            _salesByEmployeeSource.AddOrUpdate(salesByEmployees);
        }

        public async Task UpdateStastics()
        {
            if (HttpClient is null)
            {
                throw new NullReferenceException(nameof(EmployeeStasticsViewModel.HttpClient));
            }

            var newSalesByEmployeesJson = await HttpClient.GetStringAsync(BackEndEnpointURLs.EmployeeEndpoints.SalesByEmployees);
            var newSalesByEmployee = JsonSerializer.Deserialize<List<SalesByEmployeeDto>>(newSalesByEmployeesJson);

            foreach (var saleByEmployee in _salesByEmployeeSource.Items.ToList())
            {
                saleByEmployee.Sales = newSalesByEmployee.Single(x => x.ID == saleByEmployee.ID).Sales;
                _salesByEmployeeSource.Refresh(saleByEmployee);
            }
        }

        public ReadOnlyObservableCollection<SalesByEmployeeDto> SalesByEmployees => _salesByEmployee;

        public HttpClient? HttpClient { get; set; }
    }
}
