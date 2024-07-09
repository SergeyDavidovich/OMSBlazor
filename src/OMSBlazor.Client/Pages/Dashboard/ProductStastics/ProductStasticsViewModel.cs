using DynamicData;
using OMSBlazor.Client.Constants;
using OMSBlazor.Dto.Product.Stastics;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace OMSBlazor.Client.Pages.Dashboard.ProductStastics
{
    public class ProductStasticsViewModel : ReactiveObject
    {
        private readonly ReadOnlyObservableCollection<ProductsByCategoryDto> _productsByCategories;

        private readonly SourceCache<ProductsByCategoryDto, string> _productsByCategoriesSource;

        public ProductStasticsViewModel()
        {
            _productsByCategoriesSource = new(x => x.CategoryName);

            _productsByCategoriesSource
                .Connect()
                .Bind(out _productsByCategories)
                .Subscribe();
        }

        public async Task OnNavigatedTo()
        {
            if (HttpClient is null)
            {
                throw new NullReferenceException(nameof(ProductStasticsViewModel.HttpClient));
            }

            var productsByCategoriesJson = await HttpClient.GetStringAsync(BackEndEnpointURLs.ProductEndpoints.ProductByCategories);
            var productsByCategories = JsonSerializer.Deserialize<List<ProductsByCategoryDto>>(productsByCategoriesJson);

            _productsByCategoriesSource.AddOrUpdate(productsByCategories);
        }

        public ReadOnlyObservableCollection<ProductsByCategoryDto> ProductsByCategories => _productsByCategories;

        public HttpClient? HttpClient { get; set; } 
    }
}
