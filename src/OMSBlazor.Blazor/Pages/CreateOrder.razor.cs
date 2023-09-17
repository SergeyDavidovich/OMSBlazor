using AutoMapper.Internal.Mappers;
using OMSBlazor.Blazor.DTOs;
using OMSBlazor.Dto.Customer;
using OMSBlazor.Dto.Employee;
using OMSBlazor.Dto.Product;
using OMSBlazor.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OMSBlazor.Blazor.Pages
{
    public partial class CreateOrder
    {
        private readonly IProductApplicationService productApplicationService;
        private readonly ICustomerApplcationService customerApplcationService;
        private readonly IEmployeeApplicationService employeeApplicationService;

        public CreateOrder(
            IProductApplicationService productApplicationService, 
            ICustomerApplcationService customerApplcationService,
            IEmployeeApplicationService employeeApplicationService)
        {
            this.productApplicationService = productApplicationService;
            this.customerApplcationService = customerApplcationService;
            this.employeeApplicationService = employeeApplicationService;

            Employees = new ObservableCollection<EmployeeDto>();
            Customers = new ObservableCollection<CustomerDto>();
            Products = new ObservableCollection<ProductDtoObservable>();
        }

        // * Fill collection only when rendered first
        // * There is no need in re-filling Customers and Employee collection in consecutive renders because business rule don't consider that collections 
        // with this type will be manipulated 
        // * There is no need in re-filling collection of products too, state of each product will be updated by SignlaR hub
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await FillProductsAsync();
                await FillCustomersAsync();
                await FillEmployeesAsync();
            }
        }

        private async Task FillProductsAsync()
        {
            var products = await productApplicationService.GetProductsAsync();
            foreach (var product in products)
            {
                var observableProduct = new ProductDtoObservable
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Category = product.Category,
                    ReorderLevel = product.ReorderLevel,
                    Discontinued = product.Discontinued,
                    QuantityPerUnit = product.QuantityPerUnit,
                    UnitPrice = product.UnitPrice,
                    UnitsInStock = product.UnitsInStock,
                    UnitsOnOrder = product.UnitsOnOrder
                };
                Products.Add(observableProduct);
            }
        }

        private async Task FillCustomersAsync()
        {
            var customers = await customerApplcationService.GetCustomersAsync();
            foreach (var customer in customers)
            {
                Customers.Add(customer);
            }
        }

        private async Task FillEmployeesAsync()
        {
            var employees = await employeeApplicationService.GetEmployeesAsync();
            foreach(var employee in employees)
            {
                Employees.Add(employee);
            }
        }

        public ObservableCollection<EmployeeDto> Employees { get; set; }

        public ObservableCollection<ProductDtoObservable> Products { get; set; }

        public ObservableCollection<CustomerDto> Customers { get; set; }
    }
}
