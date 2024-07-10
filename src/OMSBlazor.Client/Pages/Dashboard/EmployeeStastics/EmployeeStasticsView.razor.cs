using ApexCharts;
using Microsoft.AspNetCore.Components;

namespace OMSBlazor.Client.Pages.Dashboard.EmployeeStastics
{
    public partial class EmployeeStasticsView
    {
        [Inject]
        private EmployeeStasticsViewModel EmployeeStasticsViewModel { get; set; }

        protected async override Task OnInitializedAsync()
        {
            ViewModel = EmployeeStasticsViewModel;

            var theme = new Theme
            {
                Palette = PaletteType.Palette1,
                Mode = IsDarkMode ? Mode.Dark : Mode.Light
            };
            salesByEmployeeOptions.Theme = theme;

            await ViewModel.OnNavigatedTo();

            await salesByEmployeeChart.UpdateOptionsAsync(true, true, true);
        }

        public async Task UpdateStastics()
        {
            await ViewModel.UpdateStastics();
            await salesByEmployeeChart.UpdateSeriesAsync(true);
        }
    }
}
