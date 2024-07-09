using ApexCharts;
using Microsoft.AspNetCore.Components;

namespace OMSBlazor.Client.Pages.Dashboard.ProductStastics
{
    public partial class ProductStasticsView
    {
        [Inject]
        private ProductStasticsViewModel ProductStasticsViewModel { get; set; }

        [Inject]
        private HttpClient HttpClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ViewModel = ProductStasticsViewModel;
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

            productsByCategoryOptions.Stroke = stroke;
            productsByCategoryOptions.Theme = theme;
            productsByCategoryOptions.DataLabels = new DataLabels
            {
                Enabled = true,
                Formatter = "function (val, opts) { return opts.w.config.series[opts.seriesIndex]; }"
            };

            var legend = new Legend { Position = LegendPosition.Bottom, FontSize = "15px", HorizontalAlign = ApexCharts.Align.Center };
            productsByCategoryOptions.Legend = legend;

            await ViewModel.OnNavigatedTo();

            await productsByCategoryChart.UpdateOptionsAsync(true, true, true);
        }

        public async Task UpdateStastics()
        {
            await productsByCategoryChart.UpdateOptionsAsync(true, true, true);
        }
    }
}
