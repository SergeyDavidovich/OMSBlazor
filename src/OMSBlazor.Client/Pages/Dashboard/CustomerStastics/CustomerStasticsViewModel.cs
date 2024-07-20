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
        private readonly HttpClient _httpClient;

        private readonly ReadOnlyObservableCollection<CustomersByCountryDto> _customersByCountries;
        private readonly ReadOnlyObservableCollection<PurchasesByCustomerDto> _purchasesByCustomers;

        private readonly SourceCache<PurchasesByCustomerDto, string> _purchasesByCustomersSource;
        private readonly SourceCache<CustomersByCountryDto, string> _customersByCountrySource;

        public CustomerStasticsViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;

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
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            try
            {
                var purchasesByCustomersJson = await _httpClient.GetStringAsync(BackEndEnpointURLs.CustomersEndpoints.PurchasesByCustomers);
                var customersByCountriesJson = await _httpClient.GetStringAsync(BackEndEnpointURLs.CustomersEndpoints.CustomersByCountries);

                var purchasesByCustomers = JsonSerializer.Deserialize<List<PurchasesByCustomerDto>>(purchasesByCustomersJson, options);
                var customersByCountries = JsonSerializer.Deserialize<List<CustomersByCountryDto>>(customersByCountriesJson, options);

                _purchasesByCustomersSource.AddOrUpdate(purchasesByCustomers);
                _customersByCountrySource.AddOrUpdate(customersByCountries);
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception is thrown in the {nameof(this.OnNavigatedTo)} method of the {nameof(CustomerStasticsViewModel)}", ex);
            }
        }

        public ReadOnlyObservableCollection<CustomersByCountryDto> CustomersByCountries => _customersByCountries;

        public ReadOnlyObservableCollection<PurchasesByCustomerDto> PurchasesByCustomers => _purchasesByCustomers;

        public async Task UpdateStastics()
        {
            try
            {
                var newPurchasesByCustomersJson = await _httpClient.GetStringAsync(BackEndEnpointURLs.CustomersEndpoints.PurchasesByCustomers);

                var newPurchasesByCustomers = JsonSerializer.Deserialize<List<PurchasesByCustomerDto>>(newPurchasesByCustomersJson);

                foreach (var purchasesByCustomer in _purchasesByCustomersSource.Items.ToList())
                {
                    purchasesByCustomer.Purchases = newPurchasesByCustomers.Single(x => x.CompanyName == purchasesByCustomer.CompanyName).Purchases;
                    _purchasesByCustomersSource.Refresh(purchasesByCustomer);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Exception is thrown in the {nameof(this.UpdateStastics)} method of the {nameof(CustomerStasticsViewModel)}", e);
            }
        }
    }
}
