using AutoMapper;
using OMSBlazor.Dto.Customer;
using OMSBlazor.Dto.Product;
using OMSBlazor.Northwind.CustomerAggregate;
using OMSBlazor.Northwind.ProductAggregate;

namespace OMSBlazor;

public class OMSBlazorApplicationAutoMapperProfile : Profile
{
    public OMSBlazorApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Product, ProductDto>();

        CreateMap<Customer, CustomerDto>();
    }
}
