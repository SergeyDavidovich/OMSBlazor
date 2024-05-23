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
            var stroke = new Stroke
            {
                Show = false
            };

            orderByCountriesOptions.Stroke = stroke;
            orderByCountriesOptions.Theme = theme;
            salesByCategoryOptions.Theme = theme;
            salesByCategoryOptions.Stroke = stroke;

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

            OverallSalesValue = GetSummaryValue(OMSBlazorConstants.OverallSales);
            OrdersQuantityValue = GetSummaryValue(OMSBlazorConstants.OrdersQuantity);
            AverageCheckValue = GetSummaryValue(OMSBlazorConstants.AverageCheck);
            MaxCheckValue = GetSummaryValue(OMSBlazorConstants.MaxCheck);
            MinCheckValue = GetSummaryValue(OMSBlazorConstants.MinCheck);

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
                not null => summary.SummaryValue.ToString(OMSBlazorConstants.MoneyFormat)
            };
        }

        public async Task UpdateStastics()
        {
            await ViewModel.UpdateStastics();

            await orderByCountriesChart.UpdateOptionsAsync(true, true, true);
            await salesByCategoryChart.UpdateOptionsAsync(true, true, true);
            await salesByCountriesChart.UpdateOptionsAsync(true, true, true);

            OverallSalesValue = GetSummaryValue(OMSBlazorConstants.OverallSales);
            OrdersQuantityValue = GetSummaryValue(OMSBlazorConstants.OrdersQuantity);
            AverageCheckValue = GetSummaryValue(OMSBlazorConstants.AverageCheck);
            MaxCheckValue = GetSummaryValue(OMSBlazorConstants.MaxCheck);
            MinCheckValue = GetSummaryValue(OMSBlazorConstants.MinCheck);

            // In order to update summaries
            await InvokeAsync(StateHasChanged);
        }
    }
}
