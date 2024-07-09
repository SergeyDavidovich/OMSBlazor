using DynamicData.Binding;
using DynamicData;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Text.Json;
using OMSBlazor.Client.Constants;
using OMSBlazor.Dto.Customer.Stastics;

namespace OMSBlazor.Client.Pages.Dashboard.CustomerStastics
{
    public class CustomerStasticsViewModel : ReactiveObject
    {
        private readonly ReadOnlyObservableCollection<CustomersByCountryDto> _customersByCountries;
        private readonly ReadOnlyObservableCollection<PurchasesByCustomerDto> _purchasesByCustomers;

        private readonly SourceCache<PurchasesByCustomerDto, string> _purchasesByCustomersSource;
        private readonly SourceCache<CustomersByCountryDto, string> _customersByCountrySource;

        public CustomerStasticsViewModel()
        {
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
            if (HttpClient is null)
            {
                throw new NullReferenceException(nameof(CustomerStasticsViewModel.HttpClient));
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var purchasesByCustomersJson = await HttpClient.GetStringAsync(BackEndEnpointURLs.CustomersEndpoints.PurchasesByCustomers);
            var customersByCountriesJson = await HttpClient.GetStringAsync(BackEndEnpointURLs.CustomersEndpoints.CustomersByCountries);

            var purchasesByCustomers = JsonSerializer.Deserialize<List<PurchasesByCustomerDto>>(purchasesByCustomersJson, options);
            var customersByCountries = JsonSerializer.Deserialize<List<CustomersByCountryDto>>(customersByCountriesJson, options);

            _purchasesByCustomersSource.AddOrUpdate(purchasesByCustomers);
            _customersByCountrySource.AddOrUpdate(customersByCountries);
        }

        public ReadOnlyObservableCollection<CustomersByCountryDto> CustomersByCountries => _customersByCountries;

        public ReadOnlyObservableCollection<PurchasesByCustomerDto> PurchasesByCustomers => _purchasesByCustomers;

        public HttpClient? HttpClient { get; set; }

        public async Task UpdateStastics()
        {
            if (HttpClient is null)
            {
                throw new NullReferenceException(nameof(CustomerStasticsViewModel.HttpClient));
            }

            var newPurchasesByCustomersJson = await HttpClient.GetStringAsync(BackEndEnpointURLs.CustomersEndpoints.PurchasesByCustomers);

            var newPurchasesByCustomers = JsonSerializer.Deserialize<List<PurchasesByCustomerDto>>(newPurchasesByCustomersJson);

            foreach (var purchasesByCustomer in _purchasesByCustomersSource.Items.ToList())
            {
                purchasesByCustomer.Purchases = newPurchasesByCustomers.Single(x => x.CompanyName == purchasesByCustomer.CompanyName).Purchases;
                _purchasesByCustomersSource.Refresh(purchasesByCustomer);
            }
        }
    }
}
