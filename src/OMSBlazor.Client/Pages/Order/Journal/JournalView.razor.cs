using Microsoft.AspNetCore.Components;

namespace OMSBlazor.Client.Pages.Order.Journal
{
    public partial class JournalView
    {
        [Inject]
        private JournalViewModel JournalViewModel { get; set; }

        protected async override Task OnInitializedAsync()
        {
            ViewModel = JournalViewModel;

            await ViewModel!.OnNavigatedTo();
        }
    }
}
