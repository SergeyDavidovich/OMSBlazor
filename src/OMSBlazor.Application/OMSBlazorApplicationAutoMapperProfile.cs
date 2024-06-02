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

        CreateMap<OrdersByCountry, OrdersByCountryDto>()
            .ForMember(dest => dest.CountryName, src => src.MapFrom(x => x.Key))
            .ForMember(dest => dest.OrdersCount, src => src.MapFrom(x => x.Value));

        CreateMap<SalesByCategory, SalesByCategoryDto>()
            .ForMember(dest => dest.CategoryName, src => src.MapFrom(x => x.Key))
            .ForMember(dest => dest.Sales, src => src.MapFrom(x => x.Value));

        CreateMap<SalesByCountry, SalesByCountryDto>()
            .ForMember(dest => dest.CountryName, src => src.MapFrom(x => x.Key))
            .ForMember(dest => dest.Sales, src => src.MapFrom(x => x.Value));

        CreateMap<Summary, SummaryDto>()
            .ForMember(dest => dest.SummaryName, src => src.MapFrom(x => x.Key))
            .ForMember(dest => dest.SummaryValue, src => src.MapFrom(x => x.Value));

        CreateMap<CustomersByCountry, CustomersByCountryDto>()
            .ForMember(dest => dest.CountryName, src => src.MapFrom(x => x.Key))
            .ForMember(dest => dest.CustomersCount, src => src.MapFrom(x => x.Value));

        CreateMap<PurchasesByCustomer, PurchasesByCustomerDto>()
            .ForMember(dest => dest.CompanyName, src => src.MapFrom(x => x.Key))
            .ForMember(dest => dest.Purchases, src => src.MapFrom(x => x.Value));

        CreateMap<SalesByEmployee, SalesByEmployeeDto>()
            .ForMember(dest => dest.ID, src => src.MapFrom(x => x.Key))
            .ForMember(dest => dest.Sales, src => src.MapFrom(x => x.Value));

        CreateMap<ProductsByCategory, ProductsByCategoryDto>()
            .ForMember(dest => dest.CategoryName, src => src.MapFrom(x => x.Key))
            .ForMember(dest => dest.ProductsCount, src => src.MapFrom(x => x.Value));
    }
}
