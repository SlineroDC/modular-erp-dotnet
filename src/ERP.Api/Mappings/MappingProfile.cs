using AutoMapper;
using ERP.Api.Dto;
using ERP.Core.Entities;

namespace ERP.Api.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>();

        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
    }
}
