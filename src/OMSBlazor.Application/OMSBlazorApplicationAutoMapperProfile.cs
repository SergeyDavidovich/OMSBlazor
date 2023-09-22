﻿using AutoMapper;
using OMSBlazor.Dto.Customer;
using OMSBlazor.Dto.Employee;
using OMSBlazor.Dto.Order;
using OMSBlazor.Dto.Product;
using OMSBlazor.Northwind.CustomerAggregate;
using OMSBlazor.Northwind.EmployeeAggregate;
using OMSBlazor.Northwind.OrderAggregate;
using OMSBlazor.Northwind.ProductAggregate;

namespace OMSBlazor;

public class OMSBlazorApplicationAutoMapperProfile : Profile
{
    public OMSBlazorApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Category, CategoryDto>();

        CreateMap<Product, ProductDto>();

        CreateMap<Customer, CustomerDto>()
            .ForMember(
                dest => dest.CustomerId, src => src.MapFrom(x => x.Id));

        CreateMap<Employee, EmployeeDto>();

        CreateMap<Order, OrderDto>();
    }
}
