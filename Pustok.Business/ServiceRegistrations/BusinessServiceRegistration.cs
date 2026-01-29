using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Pustok.Business.Services.Abstractions;
using Pustok.Business.Services.Implementations;
using Pustok.Business.Validators.ProductValidators;

namespace Pustok.Business.ServiceRegistrations;

public static class BusinessServiceRegistration
{

    public static void AddBusinessServices(this IServiceCollection services)
    {

        services.AddFluentValidationAutoValidation();


        services.AddValidatorsFromAssemblyContaining<ProductCreateDtoValidator>();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICloudinaryService, CloudinaryService>();
        services.AddScoped<IAuthService, AuthService>();


        services.AddAutoMapper(_ => { }, typeof(BusinessServiceRegistration).Assembly);
    }
}
