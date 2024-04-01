using Microsoft.AspNetCore.Components.Routing;
using ReactiveUI;
using System.Collections.Generic;
using System.Data.Common;
using System.Reactive;
using System;
using OMSBlazor.Application.Contracts.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using OMSBlazor.Dto.Order;

namespace OMSBlazor.Blazor.Pages.Order.Journal
{
    public class JournalViewModel : ReactiveObject
    {
        #region Declarations
        private readonly IOrderApplicationService _orderApplicationService;

        List<OrderDto> cachedCollection;
        #endregion

        #region Construct
        public JournalViewModel(IOrderApplicationService orderApplicationService)
        {
            _orderApplicationService = orderApplicationService;

            this.WhenAnyValue(x => x.SearchTerm).
                Subscribe(newSearchTerm =>
                {
                    if (newSearchTerm != null)
                        if (string.IsNullOrEmpty(newSearchTerm)) Orders = cachedCollection;
                        else
                        {
                            var filteredList = cachedCollection
                                .Where(o => o.CustomerId.Substring(0, newSearchTerm.Length).ToLower() == newSearchTerm.ToLower())
                                .OrderBy(o => o.CustomerId).ToList();

                            Orders = filteredList;
                        }
                });

            ClearSearchBoxCommand = ReactiveCommand.Create(ClearSearchBoxExecute);
        }
        #endregion

        #region Properties
        List<OrderDto> _orders;
        public List<OrderDto> Orders
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

        OrderDto _selectedOrder;
        public OrderDto SelectedOrder
        {
            get { return _selectedOrder; }
            set { this.RaiseAndSetIfChanged(ref _selectedOrder, value); }
        }
        #endregion

        public async Task OnNavigatedTo()
        {
            cachedCollection = await _orderApplicationService.GetOrdersAsync();
            Orders = cachedCollection;
        }

        #region Commands
        public ReactiveCommand<Unit, Unit> ClearSearchBoxCommand { get; }

        private void ClearSearchBoxExecute()
        {
            SearchTerm = string.Empty;
        }
        #endregion
    }
}
