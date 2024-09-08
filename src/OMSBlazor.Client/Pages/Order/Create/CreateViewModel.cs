using DynamicData.Binding;
using DynamicData;
using Microsoft.AspNetCore.SignalR.Client;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Text.Json;
using OMSBlazor.Client.Constants;
using System.Net.Http.Json;
using OMSBlazor.Dto.Employee;
using OMSBlazor.Dto.Customer;
using OMSBlazor.Dto.Product;
using OMSBlazor.Dto.Order;
using System.Text;
using MudBlazor;
using OMSBlazor.Client.Pages.Dashboard.OrderStastics;
using OMSBlazor.Client.Services.HubConnectionsService;

namespace OMSBlazor.Client.Pages.Order.Create
{
    public class CreateViewModel : ReactiveObject
    {
        #region Declarations
        private readonly HttpClient _httpClient;

        private readonly ReadOnlyObservableCollection<ProductInOrder> _productsInOrder;
        private readonly ReadOnlyObservableCollection<ProductOnStore> _productsInStore;
        private readonly ReadOnlyObservableCollection<EmployeeDto> _employees;
        private readonly ReadOnlyObservableCollection<CustomerDto> _customers;

        private readonly SourceCache<ProductOnStore, int> products;
        private readonly SourceCache<ProductInOrder, int> productsInOrder;
        private readonly SourceList<EmployeeDto> employees;
        private readonly SourceList<CustomerDto> customers;
        private List<ProductDto> productsList;

        private readonly IHubConnectionsService _hubConnectionsService;
        #endregion
        
        public CreateViewModel(HttpClient httpClient, IHubConnectionsService hubConnectionsService)
        {
            _httpClient = httpClient;
            _hubConnectionsService = hubConnectionsService;

            products = new SourceCache<ProductOnStore, int>(p => p.ProductID);
            productsInOrder = new SourceCache<ProductInOrder, int>(p => p.ProductID);
            employees = new SourceList<EmployeeDto>();
            customers = new SourceList<CustomerDto>();

            var canRemoveAllExecute = productsInOrder.CountChanged.
                Select(currentCountOfItems =>
                {
                    if (currentCountOfItems == 0) return false;
                    else return true;
                });

            productsInOrder.CountChanged.Subscribe(currentCount => { CountOfProductsInOrder = currentCount; });

            createOrderButtonDisabled = this.WhenAnyValue(vm => vm.SelectedCustomer, vm => vm.SelectedEmployee, vm => vm.CountOfProductsInOrder).
                Select(x =>
                {
                    if (x.Item1 == null || x.Item2 == null || x.Item3 == 0) return true;
                    return false;
                })
                .ToProperty(this, x => x.CreateOrderButtonDisabled);

            this.WhenAnyValue(vm => vm.SelectedCustomer, vm => vm.SelectedEmployee, vm => vm.CountOfProductsInOrder).
                Subscribe(x =>
                {
                    if (x.Item1 != null || x.Item2 != null || x.Item3 != 0) OrderDate = DateTime.Now;
                    else OrderDate = null;
                });

            products.Connect().
                OnItemAdded(a => MakeSubscribtion(a)).
                Filter(x => x.UnitsInStock != 0).
                Sort(SortExpressionComparer<ProductOnStore>.Ascending(item => item.ProductID)).
                Bind(out _productsInStore).
                Subscribe();

            productsInOrder.Connect().
                Sort(SortExpressionComparer<ProductInOrder>.Ascending(x => x.ProductID)).
                Bind(out _productsInOrder).
                OnItemAdded(x => SubscribeToChanges(x)).
                OnItemRemoved(y => RemoveProduct(y)).
                Subscribe();

            employees.Connect().
                Bind(out _employees).
                Subscribe();

            customers.Connect().
                Bind(out _customers).
                Subscribe();

            RemoveAllCommand = ReactiveCommand.Create(RemoveAllCommandExecute, canRemoveAllExecute);
            CreateOrderCommand = ReactiveCommand.CreateFromTask(CreateOrderExecute);
        }

        #region Commands

        #region Remove all 
        /// <summary>
        /// Removing all products from order
        /// </summary>
        public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> RemoveAllCommand { get; }

        private void RemoveAllCommandExecute()
        {
            var listOfProducts = products.Items.ToList();

            listOfProducts.ForEach(product => { if (product.Added) product.Added = false; });
            TotalSumString = string.Empty;
        }
        #endregion

        #region Create
        public ReactiveCommand<System.Reactive.Unit, int> CreateOrderCommand { get; }

        private async Task<int> CreateOrderExecute()
        {
            // When user clicks on "create order" button this function is runed
            // And if exception occurs, this exception will be pushed to the ThrowException observable
            // I subscribe to this observable in the code-behind so there is no need to use try-catch contstruction here
            CreateOrderDto newOrder = new CreateOrderDto();
            string shipCountry = "USA";

            newOrder.CustomerId = SelectedCustomer.CustomerId;
            newOrder.EmployeeId = SelectedEmployee.EmployeeId;
            newOrder.RequiredDate = OrderDate!.Value;
            newOrder.ShipCountry = shipCountry;

            foreach (var productInOrder in ProductsInOrder)
            {
                productInOrder.IsSaled = true;
            }

            var orderDetails = ProductsInOrder.Select(p => new OrderDetailDto
            {
                ProductId = p.ProductID,
                UnitPrice = p.UnitPrice,
                Quantity = p.SelectedQuantity,
                Discount = p.SelectedDiscount / 100
            }).ToList();

            newOrder.OrderDetails = orderDetails;

            using StringContent jsonContent = new(JsonSerializer.Serialize(newOrder), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BackEndEnpointURLs.OrderEndpoints.SaveOrder, jsonContent);
            var orderDtoJson = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var orderDto = JsonSerializer.Deserialize<OrderDto>(orderDtoJson, options);

            RemoveAllCommand.Execute().Subscribe();

            productsInOrder.Clear();
            OrderDate = null;
            SelectedCustomer = null;
            SelectedEmployee = null;

            await _httpClient.PostAsync($"{BackEndEnpointURLs.StasticsRecalculator.RecalculateStatistics}/{orderDto.OrderId}", null);
            await _hubConnectionsService.DashboardHubConnection.SendAsync("UpdateDashboard");

            return orderDto.OrderId;
        }
        #endregion

        #endregion

        #region Utilities
        /// <summary>
        /// Subscribes to the changes of added property 
        /// </summary>
        /// <param name="productOnStore">
        /// New product that was added into products on store list
        /// </param>
        private void MakeSubscribtion(ProductOnStore productOnStore)
        {
            productOnStore.WhenAnyValue(a => a.Added).
            Subscribe(isInOrder =>
            {
                if (isInOrder)
                {
                    ProductInOrder newProductInOrder = new ProductInOrder(productOnStore);
                    productsInOrder.AddOrUpdate(newProductInOrder);
                }
                else { productsInOrder.Remove(productOnStore.ProductID); }
            });
        }

        private void RemoveProduct(ProductInOrder productToRemove)
        {
            if (!productToRemove.IsSaled)
                productToRemove.SourceProductOnStore.UnitsInStock += productToRemove.SelectedQuantity;

            TotalSum -= productToRemove.Sum;
        }

        private void SubscribeToChanges(ProductInOrder newProductInOrder)
        {
            int previousSelectedQuantity = 0;

            newProductInOrder.WhenAnyValue(x => x.SelectedDiscount, x => x.SelectedQuantity)
            .Subscribe(a =>
            {
                float newSelectedDiscount = a.Item1;
                int newSelectedQuantity = a.Item2;

                //Value(price) that will be added(or removed) to(from) TotalPrice
                decimal newValue = (decimal)newProductInOrder.UnitPrice * (newSelectedQuantity - previousSelectedQuantity);

                //-1% or +1% discount
                decimal percentageOff = (decimal)(newSelectedDiscount - newProductInOrder.PreviousSelectedDiscount) / 100;

                //Executing when the SelectedDiscount has changed
                if (percentageOff != 0)
                {
                    newProductInOrder.Sum -= newProductInOrder.SelectedQuantity * (decimal)newProductInOrder.UnitPrice * percentageOff;
                    TotalSum -= newProductInOrder.SelectedQuantity * (decimal)newProductInOrder.UnitPrice * percentageOff;
                }
                //Executing when the SelectedQuantity has changed and the SelectedDiscount is greater than zero.
                else if (newSelectedDiscount != 0)
                {
                    decimal sumOff = (decimal)newProductInOrder.PreviousSelectedDiscount / 100 * newProductInOrder.SelectedQuantity * (decimal)newProductInOrder.UnitPrice;
                    decimal sumToAdd = sumOff - newProductInOrder.Sum;

                    newProductInOrder.Sum = sumOff;
                    TotalSum += sumToAdd;

                    TotalSumString = TotalSum.ToString(OMSBlazorConstants.MoneyFormat);

                    return;
                }

                newProductInOrder.PreviousSelectedDiscount = newSelectedDiscount;

                newProductInOrder.Sum += newValue;
                TotalSum += newValue;

                TotalSumString = TotalSum.ToString(OMSBlazorConstants.MoneyFormat);
            });

            newProductInOrder.WhenAnyValue(x => x.SelectedQuantity)
                .Select(newSelectedQuantity =>
                {
                    if (newSelectedQuantity == 0) return 0;
                    int deltaQuantity = newSelectedQuantity - previousSelectedQuantity; //Quantity of products that will be added or removed(in case of negative value) from the stock of the current product

                    previousSelectedQuantity = newSelectedQuantity;

                    return deltaQuantity;
                })
                .Subscribe(async newValue =>
                {
                    newProductInOrder.SourceProductOnStore.UnitsInStock -= (short)newValue;

                    var product = productsList.First(x => x.ProductId == newProductInOrder.ProductID);

                    product.UnitsInStock -= (short)newValue;
                    await _hubConnectionsService.ProductHubConnection.SendAsync("UpdateProductUnitsInStock", _hubConnectionsService.ProductHubConnection.ConnectionId, newProductInOrder.ProductID, newValue);
                });
        }
        #endregion

        /// <summary>
        /// Call this method when navigation to view occured
        /// </summary>
        /// <returns></returns>
        public async Task OnNavigatedTo()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                if (products.Count == 0)
                {
                    var productsJson = await _httpClient.GetStringAsync(BackEndEnpointURLs.ProductEndpoints.GetProducts);

                    productsList = JsonSerializer.Deserialize<List<ProductDto>>(productsJson, options);
                    var listOfProductsOnStore = productsList.Select(b => new ProductOnStore(b));
                    products.AddOrUpdate(listOfProductsOnStore);
                }
                if (employees.Count == 0)
                {
                    var employeesJson = await _httpClient.GetStringAsync(BackEndEnpointURLs.EmployeeEndpoints.GetEmployees);

                    var employeesList = JsonSerializer.Deserialize<List<EmployeeDto>>(employeesJson, options);
                    employees.AddRange(employeesList);
                }
                if (customers.Count == 0)
                {
                    var customersJson = await _httpClient.GetStringAsync(BackEndEnpointURLs.CustomersEndpoints.GetCustomers);

                    var customerList = JsonSerializer.Deserialize<List<CustomerDto>>(customersJson, options);
                    customers.AddRange(customerList);
                }

                _hubConnectionsService.ProductHubConnection.On<int, int>("UpdateQuantity", (productId, quantity) =>
                {
                    var product = ProductsInStore.Single(x => x.ProductID == productId);
                    product.UnitsInStock -= quantity;
                    this.RaisePropertyChanged(nameof(ProductsInStore));
                });

                if (_hubConnectionsService.ProductHubConnection.State is HubConnectionState.Disconnected)
                {
                    await _hubConnectionsService.ProductHubConnection.StartAsync();
                }
                if (_hubConnectionsService.DashboardHubConnection.State is HubConnectionState.Disconnected)
                {
                    await _hubConnectionsService.DashboardHubConnection.StartAsync();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Exception is thrown in the {nameof(this.OnNavigatedTo)} method of the {nameof(CreateViewModel)}", e);
            }
        }

        #region Properties
        public ReadOnlyObservableCollection<ProductInOrder> ProductsInOrder => _productsInOrder;
        public ReadOnlyObservableCollection<ProductOnStore> ProductsInStore => _productsInStore;
        public ReadOnlyObservableCollection<EmployeeDto> Employees => _employees;
        public ReadOnlyObservableCollection<CustomerDto> Customers => _customers;

        private EmployeeDto _selectedEmployee;
        public EmployeeDto SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { this.RaiseAndSetIfChanged(ref _selectedEmployee, value); }
        }

        private CustomerDto _selectedCustomer;
        public CustomerDto SelectedCustomer
        {
            get { return _selectedCustomer; }
            set { this.RaiseAndSetIfChanged(ref _selectedCustomer, value); }
        }

        private DateTime? _orderDate;
        public DateTime? OrderDate
        {
            get { return _orderDate; }
            set { this.RaiseAndSetIfChanged(ref _orderDate, value); }
        }

        private ProductInOrder _selectedProductInOrder;
        public ProductInOrder SelectedProductInOrder
        {
            get { return _selectedProductInOrder; }
            set { this.RaiseAndSetIfChanged(ref _selectedProductInOrder, value); }
        }

        private int _countOfProductsInOrder;
        public int CountOfProductsInOrder
        {
            get { return _countOfProductsInOrder; }
            set { this.RaiseAndSetIfChanged(ref _countOfProductsInOrder, value); }
        }

        string _totalSumString;
        public string TotalSumString
        {
            set { this.RaiseAndSetIfChanged(ref _totalSumString, value); }
            get { return _totalSumString; }
        }

        private readonly ObservableAsPropertyHelper<bool> createOrderButtonDisabled;
        public bool CreateOrderButtonDisabled
        {
            get { return createOrderButtonDisabled.Value; }
        }
        private decimal TotalSum { set; get; }
        #endregion

        #region Screen objects
        public class ProductOnStore : AbstractNotifyPropertyChanged
        {
            public ProductOnStore(ProductDto sourceProduct)
            {
                ProductID = sourceProduct.ProductId;
                ProductName = sourceProduct.ProductName;
                UnitPrice = sourceProduct.UnitPrice;
                UnitsInStock = sourceProduct.UnitsInStock;
                UnitsOnOrder = sourceProduct.UnitsOnOrder;
            }

            #region Properties
            public int ProductID { set; get; }

            public string ProductName { set; get; }

            public double UnitPrice { set; get; }

            int _unitsInStock;
            public int UnitsInStock
            {
                set { SetAndRaise(ref _unitsInStock, value); }
                get { return _unitsInStock; }
            }

            int _unitsOnOrder;
            public int UnitsOnOrder
            {
                set { SetAndRaise(ref _unitsOnOrder, value); }
                get { return _unitsOnOrder; }
            }

            bool _added;
            public bool Added
            {
                set { SetAndRaise(ref _added, value); }
                get { return _added; }
            }
            #endregion
        }

        public class ProductInOrder : AbstractNotifyPropertyChanged
        {
            public ProductInOrder(ProductOnStore product)
            {
                ProductID = product.ProductID;
                ProductName = product.ProductName;
                UnitPrice = product.UnitPrice;
                SelectedDiscount = 0;
                SelectedQuantity = 1;

                QunatityInStoke = product.UnitsInStock;

                SourceProductOnStore = product;
            }

            #region Properties
            public int ProductID { set; get; }

            public string ProductName { set; get; }

            public double UnitPrice { set; get; }

            public int QunatityInStoke { set; get; }

            short _selectedQuantity;
            public short SelectedQuantity
            {
                set { SetAndRaise(ref _selectedQuantity, value); }
                get { return _selectedQuantity; }
            }

            float _selectedDiscount;
            public float SelectedDiscount
            {
                set { SetAndRaise(ref _selectedDiscount, value); }
                get { return _selectedDiscount; }
            }

            public bool IsSaled { set; get; }

            public decimal Sum { set; get; }

            public ProductOnStore SourceProductOnStore { private set; get; }

            public float PreviousSelectedDiscount { set; get; }
            #endregion
        }
        #endregion
    }
}
