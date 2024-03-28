using MudBlazor;
using OMSBlazor.Blazor.Pages.Order.Create;
using OMSBlazor.Dto.Product;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using static OMSBlazor.Blazor.Pages.Order.Create.CreateViewModel;

namespace OMSBlazor.Blazor.Pages.Order
{
    public partial class Order
    {
        public Order(CreateViewModel createViewModel)
        {
            ViewModel = createViewModel;
        }

        protected async override Task OnParametersSetAsync()
        {
            await ViewModel!.OnNavigatedTo();
        }
    }
}
