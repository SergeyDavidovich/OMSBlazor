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
            var theme = new Theme
            {
                Palette = PaletteType.Palette1,
                Mode = IsDarkMode ? Mode.Dark : Mode.Light
            };
            orderByCountriesOptions.Theme = theme;
            salesByCategoryOptions.Theme = theme;

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
                },
                Theme = theme
            };

            await ViewModel!.OnNavigatedTo();

            OverallSalesValue = GetSummaryValue(OMSBlazorStasticsNames.OverallSales);
            OrdersQuantityValue = GetSummaryValue(OMSBlazorStasticsNames.OrdersQuantity);
            AverageCheckValue = GetSummaryValue(OMSBlazorStasticsNames.AverageCheck);
            MaxCheckValue = GetSummaryValue(OMSBlazorStasticsNames.MaxCheck);
            MinCheckValue = GetSummaryValue(OMSBlazorStasticsNames.MinCheck);

            StateHasChanged();

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

        public async Task UpdateStastics()
        {
            await orderByCountriesChart.UpdateOptionsAsync(true, true, true);
            await salesByCategoryChart.UpdateOptionsAsync(true, true, true);
            await salesByCountriesChart.UpdateOptionsAsync(true, true, true);
        }
    }
}
