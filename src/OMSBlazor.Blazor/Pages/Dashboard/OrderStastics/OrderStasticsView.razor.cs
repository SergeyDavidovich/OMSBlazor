using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OMSBlazor.Blazor.Pages.Dashboard.OrderStastics
{
    public partial class OrderStasticsView
    {
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
            if (summary == null)
            {
                return "No value";
            }
            else
            {
                return summary.SummaryValue.ToString();
            }
        }
    }
}
