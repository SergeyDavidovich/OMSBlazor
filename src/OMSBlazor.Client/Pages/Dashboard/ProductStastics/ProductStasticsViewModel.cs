using DynamicData;
using OMSBlazor.Client.Constants;
using OMSBlazor.Client.Pages.Dashboard.OrderStastics;
using OMSBlazor.Dto.Product.Stastics;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace OMSBlazor.Client.Pages.Dashboard.ProductStastics
{
    public class ProductStasticsViewModel : ReactiveObject
    {
        private readonly HttpClient _httpClient;

        private readonly ReadOnlyObservableCollection<ProductsByCategoryDto> _productsByCategories;

        private readonly SourceCache<ProductsByCategoryDto, string> _productsByCategoriesSource;

        public ProductStasticsViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _productsByCategoriesSource = new(x => x.CategoryName);

            _productsByCategoriesSource
                .Connect()
                .Bind(out _productsByCategories)
                .Subscribe();
        }

        public async Task OnNavigatedTo()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                var productsByCategoriesJson = await _httpClient.GetStringAsync(BackEndEnpointURLs.ProductEndpoints.ProductByCategories);
                var productsByCategories = JsonSerializer.Deserialize<List<ProductsByCategoryDto>>(productsByCategoriesJson, options);

                _productsByCategoriesSource.AddOrUpdate(productsByCategories);
            }
            catch (Exception e)
            {
                throw new Exception($"Exception is thrown in the {nameof(this.OnNavigatedTo)} method of the {nameof(ProductStasticsViewModel)}", e);
            }
        }

        public ReadOnlyObservableCollection<ProductsByCategoryDto> ProductsByCategories => _productsByCategories;
    }
}
