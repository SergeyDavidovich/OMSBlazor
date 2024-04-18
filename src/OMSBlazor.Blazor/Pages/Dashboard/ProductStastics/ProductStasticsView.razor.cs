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
            await ViewModel.OnNavigatedTo();
        }
    }
}
