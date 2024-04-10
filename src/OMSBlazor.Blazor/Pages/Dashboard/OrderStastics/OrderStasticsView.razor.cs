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

            OverallSalesValue = $"Overall sales sum - {GetSummaryValue("OverallSales")}";
            OrdersQuantityValue = $"Overall orders quantity - {GetSummaryValue("OrdersQuantity")}";
            AverageCheckValue = $"Average - {GetSummaryValue("AverageCheck")}";
            MaxCheckValue = $"Max check - {GetSummaryValue("MaxCheck")}";
            MinCheckValue = $"Min check - {GetSummaryValue("MinCheck")}";

            foreach (var salesByCountry in ViewModel.SalesByCountries)
            {
                series.Add(new() { Name = salesByCountry.CountryName, Data = new double[] { Convert.ToDouble(salesByCountry.Sales) } });
            }
        }

        List<ChartSeries> series = new List<ChartSeries>();
        string[] xLabels = new string[1] { "" };

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
