using DynamicData;
using DynamicData.Binding;
using OMSBlazor.Dto.Customer.Stastics;
using OMSBlazor.Dto.Order.Stastics;
using OMSBlazor.Interfaces.ApplicationServices;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        public async Task UpdateStastics()
        {
            Console.WriteLine("Stastic updated");
            var newPurchasesByCustomers = await _customerApplicationService.GetPurchasesByCustomer();

            foreach (var newPurchasesByCustomer in newPurchasesByCustomers)
            {
                var existingPurchasesByCustomer = _purchasesByCustomers.Single(x => x.CompanyName == newPurchasesByCustomer.CompanyName);
                existingPurchasesByCustomer.Purchases = newPurchasesByCustomer.Purchases;
            }
        }
    }
}
