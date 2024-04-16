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
            await ViewModel!.OnNavigatedTo();
        }
    }
}
