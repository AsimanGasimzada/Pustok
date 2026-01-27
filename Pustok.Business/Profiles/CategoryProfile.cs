using AutoMapper;
using Pustok.Core.Entites;

namespace Pustok.Business.Profiles;

internal class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryGetDto>().ReverseMap();
        CreateMap<Category, CategoryCreateDto>().ReverseMap();
        CreateMap<Category, CategoryUpdateDto>().ReverseMap();
    }
}
