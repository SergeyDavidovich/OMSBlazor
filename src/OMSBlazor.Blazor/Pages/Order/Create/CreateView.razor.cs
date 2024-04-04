using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace OMSBlazor.Blazor.Pages.Order.Create
{
    public partial class CreateView
    {
        public CreateView(CreateViewModel createViewModel)
        {
            ViewModel = createViewModel;
        }

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
            await ViewModel!.OnNavigatedTo();
        }
    }
}
