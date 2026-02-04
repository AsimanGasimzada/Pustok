using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Pustok.Business.Services.Abstractions;
using Pustok.Business.Services.Implementations;
using Pustok.Business.Validators.ProductValidators;
using System.Text;

namespace Pustok.Business.ServiceRegistrations;

public static class BusinessServiceRegistration
{

    public static void AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddFluentValidationAutoValidation();


        services.AddValidatorsFromAssemblyContaining<ProductCreateDtoValidator>();

        services.AddScoped<IJWTService, JWTService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICloudinaryService, CloudinaryService>();
        services.AddScoped<IAuthService, AuthService>();


        services.AddAutoMapper(_ => { }, typeof(BusinessServiceRegistration).Assembly);



        JWTOptionsDto options = configuration.GetSection("JWTOptions").Get<JWTOptionsDto>() ?? new();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(config =>
        {
            config.TokenValidationParameters = new()
            {
                RoleClaimType = "Role",
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = options.Issuer,
                ValidAudience = options.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey))
            };

        });
    }
}
