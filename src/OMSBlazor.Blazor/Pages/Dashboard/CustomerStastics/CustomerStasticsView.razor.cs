using ApexCharts;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace OMSBlazor.Blazor.Pages.Dashboard.CustomerStastics
{
    public partial class CustomerStasticsView
    {
        public CustomerStasticsView(CustomerStasticsViewModel customerStasticsViewModel)
        {
            ViewModel = customerStasticsViewModel;
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

            customersByCountriesOptions.Stroke = stroke;
            customersByCountriesOptions.Theme = theme;
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
