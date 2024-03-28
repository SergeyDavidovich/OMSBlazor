using System.Threading.Tasks;

namespace OMSBlazor.Blazor.Pages.Order.Create
{
    public partial class CreateView
    {
        public CreateView(CreateViewModel createViewModel)
        {
            ViewModel = createViewModel;
        }

        protected async override Task OnParametersSetAsync()
        {
            await ViewModel!.OnNavigatedTo();
        }
    }
}
