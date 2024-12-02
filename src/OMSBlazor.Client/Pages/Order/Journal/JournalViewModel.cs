using DynamicData.Binding;
using DynamicData;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Text.Json;
using OMSBlazor.Client.Constants;
using OMSBlazor.Dto.Order;
using System.Net.Http.Json;
using OMSBlazor.Client.Pages.Order.Create;
using OMSBlazor.Dto.Employee;
using OMSBlazor.Dto.Customer;

namespace OMSBlazor.Client.Pages.Order.Journal
{
    public class JournalViewModel : ReactiveObject
    {
        #region Declarations
        private readonly HttpClient _httpClient;

        List<SelectableOrderDto> _cachedCollection;

        SourceCache<SelectableOrderDto, int> _sourceOrders;
        #endregion

        #region Construct
        public JournalViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _cachedCollection = new List<SelectableOrderDto>();
            _sourceOrders = new SourceCache<SelectableOrderDto, int>(x => x.SourceOrderDto.OrderId);

            this.WhenAnyValue(x => x.SearchTerm).
                Subscribe(newSearchTerm =>
                {
                    if (newSearchTerm != null)
                        if (string.IsNullOrEmpty(newSearchTerm))
                        {
                            _sourceOrders.Clear();
                            _sourceOrders.AddOrUpdate(_cachedCollection);
                        }
                        else
                        {
                            var filteredList = _cachedCollection
                                .Where(o => o.SourceOrderDto.CustomerId.Substring(0, newSearchTerm.Length).ToLower() == newSearchTerm.ToLower())
                                .OrderBy(o => o.SourceOrderDto.CustomerId).ToList();

                            _sourceOrders.Clear();
                            _sourceOrders.AddOrUpdate(filteredList);
                        }
                });

            _sourceOrders.Connect()
                .Bind(out _orders)
                .Subscribe();

            ChangeSelectedOrderCommand = ReactiveCommand.CreateFromTask<int, byte[]>(ChangeSelectOrderHandler);
        }
        #endregion

        #region Properties
        ReadOnlyObservableCollection<SelectableOrderDto> _orders;
        public ReadOnlyObservableCollection<SelectableOrderDto> Orders
        {
            get { return _orders; }
            set { this.RaiseAndSetIfChanged(ref _orders, value); }
        }

        public List<EmployeeDto> Employees { get; set; }

        public List<CustomerDto> Customers { get; set; }

        string _searchTerm;
        public string SearchTerm
        {
            get { return _searchTerm; }
            set { this.RaiseAndSetIfChanged(ref _searchTerm, value); }
        }
        #endregion

        public async Task OnNavigatedTo()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                var ordersJson = await _httpClient.GetStringAsync(BackEndEnpointURLs.OrderEndpoints.GetOrders);
                var orders = JsonSerializer.Deserialize<List<OrderDto>>(ordersJson, options);

                var employeesJson = await _httpClient.GetStringAsync(BackEndEnpointURLs.EmployeeEndpoints.GetEmployees);
                Employees = JsonSerializer.Deserialize<List<EmployeeDto>>(employeesJson, options);

                var customersJson = await _httpClient.GetStringAsync(BackEndEnpointURLs.CustomersEndpoints.GetCustomers);
                Customers = JsonSerializer.Deserialize<List<CustomerDto>>(customersJson, options);

                var selectablesOrders = orders.Select(x => new SelectableOrderDto(x)).ToList();
                _cachedCollection = selectablesOrders;
                _sourceOrders.AddOrUpdate(selectablesOrders);
            }
            catch (Exception e)
            {
                throw new Exception($"Exception is thrown in the {nameof(this.OnNavigatedTo)} method of the {nameof(JournalViewModel)}", e);
            }
        }

        #region Commands
        public ReactiveCommand<int, byte[]> ChangeSelectedOrderCommand { get; }

        private async Task<byte[]> ChangeSelectOrderHandler(int orderId)
        {
            try
            {
                var previouSelectedOrder = _sourceOrders.Items.SingleOrDefault(x => x.IsSelcted);

                if (previouSelectedOrder is not null)
                {
                    previouSelectedOrder.IsSelcted = false;
                }

                var order = _sourceOrders.Items.Single(x => x.SourceOrderDto.OrderId == orderId);
                order.IsSelcted = true;

                var response = await _httpClient.GetAsync(BackEndEnpointURLs.OrderEndpoints.GetUrlForInvoice(order.SourceOrderDto.OrderId));
                var arr = await response.Content.ReadFromJsonAsync<byte[]>();

                return arr;
            }
            catch (Exception e)
            {
                throw new Exception($"Exception is thrown in the {nameof(this.ChangeSelectedOrderCommand)} method of the {nameof(JournalViewModel)}", e);
            }
        }
        #endregion

        #region Utilities
        public async Task<string> GetCheckoutUrl(int orderId, string domain, string currency = "usd")
        {
            var order = _cachedCollection.Single(x => x.SourceOrderDto.OrderId == orderId);
            var price = order.SourceOrderDto.OrderDetails.Sum(x => x.UnitPrice * x.Quantity);
            var checkoutUrl = await _httpClient.GetStringAsync($"{StripeModule.Pages.BackEndEndpointURLs.GetCheckoutUrl}/{orderId}?price={price}&domain={domain}&currency={currency}");
            return checkoutUrl;
        }
        #endregion
    }

    public class SelectableOrderDto : AbstractNotifyPropertyChanged
    {
        public SelectableOrderDto(OrderDto orderDto)
        {
            SourceOrderDto = orderDto;
        }

        public OrderDto SourceOrderDto { get; }

        private bool _isSelected;
        public bool IsSelcted
        {
            get { return _isSelected; }
            set { SetAndRaise(ref _isSelected, value); }
        }
    }
}
