using ApexCharts;
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
            var legend = new Legend { Position = LegendPosition.Bottom, FontSize = "15px", HorizontalAlign = ApexCharts.Align.Center };
            customersByCountriesOptions.Legend = legend;

            await ViewModel!.OnNavigatedTo();

            await purchasesByCustomersChart.UpdateOptionsAsync(true, true, true);
            await customersByCountriesChart.UpdateOptionsAsync(true, true, true);
        }
    }
}
