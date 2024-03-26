using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Binding;
using System.Reactive.Linq;
using System.Reactive;
using System.Data.Common;
using System.Runtime.InteropServices;
using DynamicData.Kernel;
using Microsoft.AspNetCore.Components.Routing;
using NUglify.Helpers;
using OMSBlazor.Dto.Employee;
using OMSBlazor.Dto.Customer;
using OMSBlazor.Dto.Product;
using OMSBlazor.Dto.Order;
using OMSBlazor.Application.Contracts.Interfaces;

namespace OMSBlazor.Blazor.Pages.Order.Create
{
    public class CreateViewModel : ReactiveObject
    {
        #region Declarations
        private readonly IEmployeeApplicationService _employeeApplicationService;
        private readonly IOrderApplicationService _orderApplicationService;
        private readonly IProductApplicationService _productApplicationService;
        private readonly ICustomerApplcationService _customerApplcationService;

        private readonly ReadOnlyObservableCollection<ProductInOrder> _productsInOrder;
        private readonly ReadOnlyObservableCollection<ProductOnStore> _productsInStore;
        private readonly ReadOnlyObservableCollection<EmployeeDto> _employees;
        private readonly ReadOnlyObservableCollection<CustomerDto> _customers;

        private readonly SourceCache<ProductOnStore, int> products;
        private readonly SourceCache<ProductInOrder, int> productsInOrder;
        private readonly SourceList<EmployeeDto> employees;
        private readonly SourceList<CustomerDto> customers;

        private List<ProductDto> productsList;
        #endregion

        public CreateViewModel(
            IEmployeeApplicationService employeeApplicationService,
            IOrderApplicationService orderApplicationService,
            IProductApplicationService productApplicationService,
            ICustomerApplcationService customerApplcationService)
        {
            _employeeApplicationService = employeeApplicationService;
            _orderApplicationService = orderApplicationService;
            _productApplicationService = productApplicationService;
            _customerApplcationService = customerApplcationService;

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

            var canCreateOrderExecute = this.WhenAnyValue(vm => vm.SelectedCustomer, vm => vm.SelectedEmployee, vm => vm.CountOfProductsInOrder).
                Select(x =>
                {
                    if (x.Item1 == null || x.Item2 == null || x.Item3 == 0) return false;
                    return true;
                });

            this.WhenAnyValue(vm => vm.SelectedCustomer, vm => vm.SelectedEmployee, vm => vm.CountOfProductsInOrder).
                Subscribe(x =>
                {
                    if (x.Item1 != null || x.Item2 != null || x.Item3 != 0) OrderDate = DateTime.Now.ToLongDateString();
                    else OrderDate = "";
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
            CreateOrderCommand = ReactiveCommand.Create(CreateOrderExecute, canCreateOrderExecute);
        }

        #region Commands

        #region Remove all 
        /// <summary>
        /// Removing all products from order
        /// </summary>
        public ReactiveCommand<Unit, Unit> RemoveAllCommand { get; }

        private void RemoveAllCommandExecute()
        {
            var listOfProducts = products.Items.ToList();

            listOfProducts.ForEach(product => { if (product.Added) product.Added = false; });
        }
        #endregion

        #region Create
        public ReactiveCommand<Unit, Unit> CreateOrderCommand { get; }

        private async void CreateOrderExecute()
        {
            CreateOrderDto newOrder = new CreateOrderDto();
            string shipCountry = "USA";

            newOrder.CustomerId = SelectedCustomer.CustomerId;
            newOrder.EmployeeId = SelectedEmployee.EmployeeId;
            newOrder.RequiredDate = DateTime.Parse(OrderDate);
            newOrder.ShipCountry = shipCountry;

            ProductsInOrder.ForEach(productInOrder => productInOrder.IsSaled = true);

            var orderDetails = ProductsInOrder.Select(p => new OrderDetailDto
            {
                ProductId = p.ProductID,
                UnitPrice = p.UnitPrice,
                Quantity = p.SelectedQuantity,
                Discount = p.SelectedDiscount / 100
            }).ToList();

            await _orderApplicationService.SaveOrderAsync(newOrder);

            RemoveAllCommand.Execute().Subscribe();

            productsInOrder.Clear();
            OrderDate = string.Empty;
            SelectedCustomer = null;
            SelectedEmployee = null;
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

                    TotalSumString = TotalSum.ToString("C2");

                    return;
                }

                newProductInOrder.PreviousSelectedDiscount = newSelectedDiscount;

                newProductInOrder.Sum += newValue;
                TotalSum += newValue;

                TotalSumString = TotalSum.ToString("C2");
            });

            newProductInOrder.WhenAnyValue(x => x.SelectedQuantity)
                .Select(newSelectedQuantity =>
                {
                    if (newSelectedQuantity == 0) return 0;
                    int deltaQuantity = newSelectedQuantity - previousSelectedQuantity; //Quantity of products that will be added or removed(in case of negative value) from the stock of the current product

                    previousSelectedQuantity = newSelectedQuantity;

                    return deltaQuantity;
                })
                .Subscribe(newValue =>
                {
                    newProductInOrder.SourceProductOnStore.UnitsInStock -= (short)newValue;

                    ProductDto productToReplace = productsList.First(x => x.ProductId == newProductInOrder.ProductID);

                    productToReplace.UnitsInStock -= (short)newValue;
                });
        }
        #endregion

        #region Implementation of INavigationAware
        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (products.Count == 0)
            {
                productsList = new List<ProductDto>(await _productApplicationService.GetProductsAsync());
                var listOfProductsOnStore = productsList.Select(b => new ProductOnStore(b));
                products.AddOrUpdate(listOfProductsOnStore);
            }
            if (employees.Count == 0)
            {
                var employeesList = await _employeeApplicationService.GetEmployeesAsync();
                employees.AddRange(employeesList);
            }
            if (customers.Count == 0)
            {
                var customerList = await _customerApplcationService.GetCustomersAsync();
                customers.AddRange(customerList);
            }
        }
        #endregion

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

        private string _orderDate;
        public string OrderDate
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