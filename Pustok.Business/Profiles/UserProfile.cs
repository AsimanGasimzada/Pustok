using AutoMapper;
using Pustok.Business.Dtos.UserDtos;
using Pustok.Core.Entites;

namespace Pustok.Business.Profiles;

internal class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AppUser, RegisterDto>().ReverseMap();
    }
}
