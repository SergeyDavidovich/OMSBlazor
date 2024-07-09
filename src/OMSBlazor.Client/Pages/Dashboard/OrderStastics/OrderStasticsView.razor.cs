using ApexCharts;
using Microsoft.AspNetCore.Components;
using OMSBlazor.Dto.Order.Stastics;

namespace OMSBlazor.Client.Pages.Dashboard.OrderStastics
{
    public partial class OrderStasticsView
    {
        [Inject]
        private OrderStasticsViewModel OrderStasticsViewModel { get; set; }

        [Inject]
        private HttpClient HttpClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ViewModel = OrderStasticsViewModel;
            ViewModel.HttpClient = HttpClient;

            var theme = new Theme
            {
                Palette = PaletteType.Palette1,
                Mode = IsDarkMode ? Mode.Dark : Mode.Light
            };
            var stroke = new Stroke
            {
                Show = false
            };
            var dataLables = new DataLabels
            {
                Enabled = true,
                Formatter = "function (val, opts) { return opts.w.config.series[opts.seriesIndex]; }"
            };

            orderByCountriesOptions.Stroke = stroke;
            orderByCountriesOptions.Theme = theme;
            salesByCategoryOptions.Theme = theme;
            salesByCategoryOptions.Stroke = stroke;

            var legend = new Legend { Position = LegendPosition.Bottom, FontSize = "15px", HorizontalAlign = ApexCharts.Align.Center };
            orderByCountriesOptions.Legend = legend;
            orderByCountriesOptions.DataLabels = dataLables;
            salesByCategoryOptions.Legend = legend;
            salesByCategoryOptions.DataLabels = dataLables;
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
