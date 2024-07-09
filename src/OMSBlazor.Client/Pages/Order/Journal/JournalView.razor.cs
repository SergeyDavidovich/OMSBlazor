namespace OMSBlazor.Client.Pages.Order.Journal
{
    public partial class JournalView
    {
        public JournalView(JournalViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        protected async override Task OnInitializedAsync()
        {
            await ViewModel!.OnNavigatedTo();
        }
    }
}
