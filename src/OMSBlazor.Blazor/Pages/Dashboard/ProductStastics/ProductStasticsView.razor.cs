using ApexCharts;
using System.Threading.Tasks;

namespace OMSBlazor.Blazor.Pages.Dashboard.ProductStastics
{
    public partial class ProductStasticsView
    {
        public ProductStasticsView(ProductStasticsViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        protected override async Task OnInitializedAsync()
        {
            var legend = new Legend { Position = LegendPosition.Bottom, FontSize = "15px", HorizontalAlign = ApexCharts.Align.Center };
            productsByCategoryOptions.Legend = legend;

            await ViewModel.OnNavigatedTo();

            await productsByCategoryChart.UpdateOptionsAsync(true, true, true);
        }
    }
}
