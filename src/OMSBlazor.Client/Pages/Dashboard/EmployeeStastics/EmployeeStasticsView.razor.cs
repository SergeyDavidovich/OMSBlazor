using ApexCharts;

namespace OMSBlazor.Client.Pages.Dashboard.EmployeeStastics
{
    public partial class EmployeeStasticsView
    {
        public EmployeeStasticsView(EmployeeStasticsViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        protected async override Task OnInitializedAsync()
        {
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
