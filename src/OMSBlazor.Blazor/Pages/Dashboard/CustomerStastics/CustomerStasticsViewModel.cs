using DynamicData;
using DynamicData.Binding;
using OMSBlazor.Application.Contracts.Interfaces;
using OMSBlazor.Dto.Customer.Stastics;
using OMSBlazor.Dto.Order.Stastics;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OMSBlazor.Blazor.Pages.Dashboard.CustomerStastics
{
    public class CustomerStasticsViewModel : ReactiveObject
    {
        private readonly ICustomerApplcationService _customerApplicationService;

        private readonly ReadOnlyObservableCollection<CustomersByCountryDto> _customersByCountries;
        private readonly ReadOnlyObservableCollection<PurchasesByCustomerDto> _purchasesByCustomers;

        private readonly SourceCache<PurchasesByCustomerDto, string> _purchasesByCustomersSource;
        private readonly SourceCache<CustomersByCountryDto, string> _customersByCountrySource;

        public CustomerStasticsViewModel(ICustomerApplcationService customerApplicationService)
        {
            _customerApplicationService = customerApplicationService;

            _customersByCountrySource = new(x => x.CountryName);
            _purchasesByCustomersSource = new(x => x.CompanyName);

            _customersByCountrySource.Connect()
                .Bind(out _customersByCountries)
                .Subscribe();

            _purchasesByCustomersSource.Connect()
                .Sort(SortExpressionComparer<PurchasesByCustomerDto>.Descending(x => x.Purchases))
                .Top(10)
                .Bind(out _purchasesByCustomers)
                .Subscribe();
        }

        public async Task OnNavigatedTo()
        {
            var purchasesByCustomers = await _customerApplicationService.GetPurchasesByCustomer();
            var customersByCountries = await _customerApplicationService.GetCustomersByCountry();

            _purchasesByCustomersSource.AddOrUpdate(purchasesByCustomers);
            _customersByCountrySource.AddOrUpdate(customersByCountries);
        }

        public ReadOnlyObservableCollection<CustomersByCountryDto> CustomersByCountries => _customersByCountries;

        public ReadOnlyObservableCollection<PurchasesByCustomerDto> PurchasesByCustomers => _purchasesByCustomers;
    }
}
