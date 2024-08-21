using Microsoft.AspNetCore.Components;
using MudBlazor;
using OMSBlazor.Client.Components;
using System.Reactive.Linq;

namespace OMSBlazor.Client.Pages.Order.Create
{
    public partial class CreateView
    {
        //https://stackoverflow.com/questions/77159834/missingmethodexception-cannot-dynamically-create-an-instance-of-type-with-blazo
        [Inject]
        private CreateViewModel CreateViewModel { get; set; }

        [Inject]
        ISnackbar Snackbar { get; set; }

        [Inject]
        IDialogService DialogService { get; set; }

        private async Task CreateOrderButtonClicked()
        {
            var result = await ViewModel!.CreateOrderCommand.Execute();

            Snackbar.Add("Order created successfully", Severity.Success);

            await PdfGenerated.InvokeAsync(result);
        }

        private async Task RemoveAllButtonClicked()
        {
            await ViewModel!.RemoveAllCommand.Execute();
        }

        protected async override Task OnInitializedAsync()
        {
            ViewModel = CreateViewModel;
            ViewModel.CreateOrderCommand.ThrownExceptions.Subscribe(async e =>
            {
                var parameters = new DialogParameters<ErrorMessageBox> { { x => x.ErrorMessage, e.Message } };
                await DialogService.ShowAsync<ErrorMessageBox>("Order not created", parameters);
            });
            await ViewModel!.OnNavigatedTo();
        }
    }
}
