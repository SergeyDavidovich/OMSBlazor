using System.Threading.Tasks;

namespace OMSBlazor.Blazor.Pages.Order.Journal
{
    public partial class JournalView
    {
        public JournalView(JournalViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        protected async override Task OnInitializedAsync()
        {
            await ViewModel!.OnNavigatedTo();
        }
    }
}
