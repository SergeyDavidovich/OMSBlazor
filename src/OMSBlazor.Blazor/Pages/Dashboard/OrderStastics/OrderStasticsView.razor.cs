using ApexCharts;
using MudBlazor;
using OMSBlazor.Dto.Order.Stastics;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection.Emit;
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
            var legend = new Legend { Position = LegendPosition.Bottom, FontSize = "15px", HorizontalAlign = ApexCharts.Align.Center };
            orderByCountriesOptions.Legend = legend;
            salesByCategoryOptions.Legend = legend;
            salesByCountriesOptions = new ApexChartOptions<SalesByCountryDto>
            {
                PlotOptions = new PlotOptions
                {
                    Bar = new PlotOptionsBar
                    {
                        Horizontal = true
                    }
                }
            };

            await ViewModel!.OnNavigatedTo();

            OverallSalesValue = GetSummaryValue("OverallSales");
            OrdersQuantityValue = GetSummaryValue("OrdersQuantity");
            AverageCheckValue = GetSummaryValue("AverageCheck");
            MaxCheckValue = GetSummaryValue("MaxCheck");
            MinCheckValue = GetSummaryValue("MinCheck");

            await orderByCountriesChart.UpdateOptionsAsync(true, true, true);
            await salesByCategoryChart.UpdateOptionsAsync(true, true, true);
            await salesByCountriesChart.UpdateOptionsAsync(true, true, true);
        }

        string GetSummaryValue(string summaryName)
        {
            var summary = ViewModel.Summaries.SingleOrDefault(x => x.SummaryName == summaryName);
            return summary switch
            {
                null => "No value",
                not null when summaryName == "OrdersQuantity" => summary.SummaryValue.ToString(),
                not null => summary.SummaryValue.ToString(format)
            };
        }
    }
}
