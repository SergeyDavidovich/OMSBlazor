using ApexCharts;
using Microsoft.AspNetCore.Components;

namespace OMSBlazor.Client.Pages.Dashboard.CustomerStastics
{
    public partial class CustomerStasticsView
    {
        [Inject]
        private CustomerStasticsViewModel CustomerStasticsViewModel { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ViewModel = CustomerStasticsViewModel;

            var theme = new Theme
            {
                Palette = PaletteType.Palette1,
                Mode = IsDarkMode ? Mode.Dark : Mode.Light
            };
            var stroke = new Stroke
            {
                Show = false
            };

            customersByCountriesOptions.Stroke = stroke;
            customersByCountriesOptions.Theme = theme;
            customersByCountriesOptions.DataLabels = new DataLabels
            {
                Enabled = true,
                Formatter = "function (val, opts) { return opts.w.config.series[opts.seriesIndex]; }"
            };
            purchasesByCustomersOptions.Theme = theme;

            var legend = new Legend { Position = LegendPosition.Bottom, FontSize = "15px", HorizontalAlign = ApexCharts.Align.Center };
            customersByCountriesOptions.Legend = legend;

            await ViewModel!.OnNavigatedTo();

            await purchasesByCustomersChart.UpdateOptionsAsync(true, true, true);
            await customersByCountriesChart.UpdateOptionsAsync(true, true, true);
        }

        public async Task UpdateStastics()
        {
            await ViewModel.UpdateStastics();
            await purchasesByCustomersChart.UpdateSeriesAsync(true);
        }
    }
}
