using Microsoft.AspNetCore.Components.Routing;
using ReactiveUI;
using System.Collections.Generic;
using System.Data.Common;
using System.Reactive;
using System;
using System.Threading.Tasks;
using System.Linq;
using OMSBlazor.Dto.Order;
using OMSBlazor.Interfaces.ApplicationServices;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Binding;
using System.Reactive.Linq;

namespace OMSBlazor.Blazor.Pages.Order.Journal
{
    public class JournalViewModel : ReactiveObject
    {
        #region Declarations
        private readonly IOrderApplicationService _orderApplicationService;

        List<SelectableOrderDto> _cachedCollection;

        SourceCache<SelectableOrderDto, int> _sourceOrders;
        #endregion

        #region Construct
        public JournalViewModel(IOrderApplicationService orderApplicationService)
        {
            _orderApplicationService = orderApplicationService;
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
            var selectablesOrders = (await _orderApplicationService.GetOrdersAsync()).Select(x => new SelectableOrderDto(x)).ToList();
            _cachedCollection = selectablesOrders;
            _sourceOrders.AddOrUpdate(selectablesOrders);
        }

        #region Commands
        public ReactiveCommand<int, byte[]> ChangeSelectedOrderCommand { get; }

        private async Task<byte[]> ChangeSelectOrderHandler(int orderId)
        {
            var previouSelectedOrder = _sourceOrders.Items.SingleOrDefault(x => x.IsSelcted);

            if (previouSelectedOrder is not null)
            {
                previouSelectedOrder.IsSelcted = false;
            }

            var order = _sourceOrders.Items.Single(x => x.SourceOrderDto.OrderId == orderId);
            order.IsSelcted = true;

            var arr = await _orderApplicationService.GetInvoiceAsync(orderId);

            return arr;
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
