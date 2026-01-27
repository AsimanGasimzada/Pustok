using AutoMapper;
using Pustok.Core.Entites;

namespace Pustok.Business.Profiles;

internal class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductCreateDto>().ReverseMap();
        CreateMap<Product, ProductGetDto>().ReverseMap();
        CreateMap<Product, ProductUpdateDto>().ReverseMap();
    }
}
