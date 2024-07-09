using Microsoft.AspNetCore.Components;
using System.Reactive.Linq;

namespace OMSBlazor.Client.Pages.Order.Create
{
    public partial class CreateView : IAsyncDisposable
    {
        //https://stackoverflow.com/questions/77159834/missingmethodexception-cannot-dynamically-create-an-instance-of-type-with-blazo
        [Inject]
        private CreateViewModel CreateViewModel { get; set; }

        [Inject]
        private HttpClient HttpClient { get; set; }

        private async Task CreateOrderButtonClicked()
        {
            var result = await ViewModel!.CreateOrderCommand.Execute();

            await PdfGenerated.InvokeAsync(result);
        }

        private async Task RemoveAllButtonClicked()
        {
            await ViewModel!.RemoveAllCommand.Execute();
        }

        protected async override Task OnInitializedAsync()
        {
            ViewModel = CreateViewModel;
            ViewModel.HttpClient = HttpClient;
            await ViewModel!.OnNavigatedTo();
        }

        public async ValueTask DisposeAsync()
        {
            await ViewModel!.DisposeAsync();
        }
    }
}
