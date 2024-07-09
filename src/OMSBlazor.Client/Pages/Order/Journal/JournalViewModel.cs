using DynamicData.Binding;
using DynamicData;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Text.Json;
using OMSBlazor.Client.Constants;
using OMSBlazor.Dto.Order;
using System.Net.Http.Json;

namespace OMSBlazor.Client.Pages.Order.Journal
{
    public class JournalViewModel : ReactiveObject
    {
        #region Declarations
        List<SelectableOrderDto> _cachedCollection;

        SourceCache<SelectableOrderDto, int> _sourceOrders;
        #endregion

        #region Construct
        public JournalViewModel()
        {
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

        string _searchTerm;
        public string SearchTerm
        {
            get { return _searchTerm; }
            set { this.RaiseAndSetIfChanged(ref _searchTerm, value); }
        }
        #endregion

        public async Task OnNavigatedTo()
        {
            if (HttpClient is null)
            {
                throw new NullReferenceException(nameof(JournalViewModel.HttpClient));
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var ordersJson = await HttpClient.GetStringAsync(BackEndEnpointURLs.OrderEndpoints.GetOrders);
            var orders = JsonSerializer.Deserialize<List<OrderDto>>(ordersJson, options);

            var selectablesOrders = orders.Select(x => new SelectableOrderDto(x)).ToList();
            _cachedCollection = selectablesOrders;
            _sourceOrders.AddOrUpdate(selectablesOrders);
        }

        #region Commands
        public ReactiveCommand<int, byte[]> ChangeSelectedOrderCommand { get; }

        private async Task<byte[]> ChangeSelectOrderHandler(int orderId)
        {
            if (HttpClient is null)
            {
                throw new NullReferenceException(nameof(JournalViewModel.HttpClient));
            }

            var previouSelectedOrder = _sourceOrders.Items.SingleOrDefault(x => x.IsSelcted);

            if (previouSelectedOrder is not null)
            {
                previouSelectedOrder.IsSelcted = false;
            }

            var order = _sourceOrders.Items.Single(x => x.SourceOrderDto.OrderId == orderId);
            order.IsSelcted = true;

            var response = await HttpClient.GetAsync(BackEndEnpointURLs.OrderEndpoints.GetUrlForInvoice(order.SourceOrderDto.OrderId));
            var arr = await response.Content.ReadFromJsonAsync<byte[]>();

            return arr;
        }
        #endregion

        public HttpClient? HttpClient { get; set; }
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
