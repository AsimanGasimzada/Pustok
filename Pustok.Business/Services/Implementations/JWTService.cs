using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pustok.Business.Services.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pustok.Business.Services.Implementations;

internal class JWTService : IJWTService
{
    private readonly JWTOptionsDto _options;
    public JWTService(IConfiguration configuration)
    {
        _options = configuration.GetSection("JWTOptions").Get<JWTOptionsDto>() ?? new();
    }

    public AccessTokenDto CreateAccessToken(List<Claim> claims)
    {

        string secretKey = _options.SecretKey;

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(secretKey));

        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        JwtHeader jwtHeader = new(signingCredentials);

        //Name:elchin\
        //Role:member
        //Age:20
        //ExpiredDate
        //Date


        JwtPayload payload = new(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiredDate));


        JwtSecurityToken securityToken = new(jwtHeader, payload);

        JwtSecurityTokenHandler handler = new();

        string token = handler.WriteToken(securityToken);


        string refreshToken = Guid.NewGuid().ToString();

        return new()
        {
            Token = token,
            ExpiredDate = DateTime.UtcNow.AddMinutes(_options.ExpiredDate),
            RefreshToken = refreshToken,
            RefreshTokenExpiredDate = DateTime.UtcNow.AddMinutes(_options.ExpiredDate + 60)
        };
    }
}
