using System.Threading.Tasks;

namespace OMSBlazor.Blazor.Pages.Dashboard.EmployeeStastics
{
    public partial class EmployeeStasticsView
    {
        public EmployeeStasticsView(EmployeeStasticsViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        protected async override Task OnInitializedAsync()
        {
            await ViewModel.OnNavigatedTo();
        }
    }
}
