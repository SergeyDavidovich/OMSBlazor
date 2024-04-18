using AutoMapper;
using OMSBlazor.Dto.Category;
using OMSBlazor.Dto.Customer;
using OMSBlazor.Dto.Customer.Stastics;
using OMSBlazor.Dto.Employee;
using OMSBlazor.Dto.Employee.Stastics;
using OMSBlazor.Dto.Order;
using OMSBlazor.Dto.Order.Stastics;
using OMSBlazor.Dto.Product;
using OMSBlazor.Dto.Product.Stastics;
using OMSBlazor.Northwind.OrderAggregate;
using OMSBlazor.Northwind.Stastics;

namespace OMSBlazor;

public class OMSBlazorApplicationAutoMapperProfile : Profile
{
    public OMSBlazorApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Category, CategoryDto>();

        CreateMap<Product, ProductDto>()
            .ForMember(
                dest => dest.ProductId, src => src.MapFrom(x => x.Id));

        CreateMap<Customer, CustomerDto>()
            .ForMember(
                dest => dest.CustomerId, src => src.MapFrom(x => x.Id));

        CreateMap<Employee, EmployeeDto>()
            .ForMember(
                dest => dest.EmployeeId, src => src.MapFrom(x => x.Id));

        CreateMap<Order, OrderDto>()
            .ForMember(
                dest => dest.OrderId, src => src.MapFrom(x => x.Id));

        CreateMap<OrderDetail, OrderDetailDto>();

        CreateMap<OrdersByCountry, OrdersByCountryDto>();

        CreateMap<SalesByCategory, SalesByCategoryDto>();

        CreateMap<SalesByCountry, SalesByCountryDto>();

        CreateMap<Summary, SummaryDto>();

        CreateMap<CustomersByCountry, CustomersByCountryDto>();

        CreateMap<PurchasesByCustomer, PurchasesByCustomerDto>();

        CreateMap<SalesByEmployee, SalesByEmployeeDto>();

        CreateMap<ProductsByCategory, ProductsByCategoryDto>();
    }
}
