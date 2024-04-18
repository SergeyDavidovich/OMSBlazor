using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OMSBlazor.Blazor.Pages.Dashboard.OrderStastics
{
    public partial class OrderStasticsView
    {
        private readonly string format = "$ ###,###.###";
        public OrderStasticsView(OrderStasticsViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        protected override async Task OnInitializedAsync()
        {
            await ViewModel!.OnNavigatedTo();

            OverallSalesValue = GetSummaryValue("OverallSales");
            OrdersQuantityValue = GetSummaryValue("OrdersQuantity");
            AverageCheckValue = GetSummaryValue("AverageCheck");
            MaxCheckValue = GetSummaryValue("MaxCheck");
            MinCheckValue = GetSummaryValue("MinCheck");
        }

        string GetSummaryValue(string summaryName)
        {
            var summary = ViewModel.Summaries.SingleOrDefault(x => x.SummaryName == summaryName);
            return summary switch
            {
                null => "No value",
                not null when summaryName== "OrdersQuantity" => summary.SummaryValue.ToString(),
                not null => summary.SummaryValue.ToString(format)
            };
        }
    }
}
