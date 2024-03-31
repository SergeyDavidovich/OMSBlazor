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
            await ViewModel!.CreateOrderCommand.Execute();
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
