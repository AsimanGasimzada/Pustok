using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pustok.Business.Dtos.UserDtos;
using Pustok.Business.Exceptions;
using Pustok.Business.Services.Abstractions;
using Pustok.Core.Entites;
using Pustok.Core.Enums;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace Pustok.Business.Services.Implementations;

internal class AuthService(UserManager<AppUser> _userManager, IMapper _mapper, IJWTService _jWTService) : IAuthService
{
    public async Task<ResultDto<AccessTokenDto>> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.EmailOrUsername);

        if (user is null)
        {
            user = await _userManager.FindByNameAsync(dto.EmailOrUsername);

            if (user is null)
            {
                throw new LoginFailException();
            }
        }

        var isTruePassword = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!isTruePassword)
            throw new LoginFailException();

        AccessTokenDto tokenResult = await _GetAccessTokenAsync(user);

        return new ResultDto<AccessTokenDto>(tokenResult);

    }

    private async Task<AccessTokenDto> _GetAccessTokenAsync(AppUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        List<Claim> claims = [
            new("Fullname",user.Fullname),
            new("Username",user.UserName!),
            new("Email",user.Email!),
            new("Role",roles.FirstOrDefault() ?? ""),
            ];


        var tokenResult = _jWTService.CreateAccessToken(claims);


        user.RefreshToken = tokenResult.RefreshToken;
        user.RefreshTokenExpiredDate = tokenResult.RefreshTokenExpiredDate;

        await _userManager.UpdateAsync(user);
        return tokenResult;
    }

    public async Task<ResultDto<AccessTokenDto>> RefreshTokenAsync(string token)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == token && x.RefreshTokenExpiredDate > DateTime.UtcNow);

        if (user is null)
            throw new LoginFailException();

        var tokenResult = await _GetAccessTokenAsync(user);

        return new(tokenResult);
    }

    public async Task<ResultDto> RegisterAsync(RegisterDto dto)
    {

        var isExistUsername = await _userManager.Users.AnyAsync(x => x.UserName!.ToLower() == dto.UserName.ToLower());


        if (isExistUsername)
            throw new AlreadyExistException("This username is already exist");

        var isExistEmail = await _userManager.Users.AnyAsync(x => x.Email!.ToLower() == dto.Email.ToLower());


        if (isExistEmail)
            throw new AlreadyExistException("This email is already exist");


        var user = _mapper.Map<AppUser>(dto);


        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            string errorMessage = string.Join(", \n", result.Errors.Select(e => e.Description));

            throw new RegisterFailException(errorMessage);
        }


        await _userManager.AddToRoleAsync(user, IdentityRoles.Member.ToString());

        return new();

    }
}
