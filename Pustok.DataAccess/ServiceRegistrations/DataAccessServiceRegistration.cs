using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pustok.Core.Entites;
using Pustok.DataAccess.Abstractions;
using Pustok.DataAccess.ContextInitalizers;
using Pustok.DataAccess.Contexts;
using Pustok.DataAccess.Interceptors;
using Pustok.DataAccess.Repositories.Abstractions;
using Pustok.DataAccess.Repositories.Implementations;

namespace Pustok.DataAccess.ServiceRegistrations;

public static class DataAccessServiceRegistration
{

    public static void AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<IContextInitalizer, DbContextInitalizer>();

        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.Password.RequiredLength = 5;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireDigit = false;

            options.User.RequireUniqueEmail = true;

        }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Default"));
        });

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<BaseAuditableInterceptor>();
    }
}
