using DynamicData;
using OMSBlazor.Application.Contracts.Interfaces;
using OMSBlazor.Dto.Product.Stastics;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OMSBlazor.Blazor.Pages.Dashboard.ProductStastics
{
    public class ProductStasticsViewModel : ReactiveObject
    {
        private readonly IProductApplicationService _productApplicationService;

        private readonly ReadOnlyObservableCollection<ProductsByCategoryDto> _productsByCategories;

        private readonly SourceCache<ProductsByCategoryDto, string> _productsByCategoriesSource;

        public ProductStasticsViewModel(IProductApplicationService productApplicationService)
        {
            _productApplicationService = productApplicationService;

            _productsByCategoriesSource = new(x => x.CategoryName);

            _productsByCategoriesSource
                .Connect()
                .Bind(out _productsByCategories)
                .Subscribe();
        }

        public async Task OnNavigatedTo()
        {
            var productsByCategories = await _productApplicationService.GetProductsByCategoryAsync();

            _productsByCategoriesSource.AddOrUpdate(productsByCategories);
        }

        public ReadOnlyObservableCollection<ProductsByCategoryDto> ProductsByCategories => _productsByCategories;
    }
}
