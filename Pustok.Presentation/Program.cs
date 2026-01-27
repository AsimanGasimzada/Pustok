using Pustok.DataAccess.ServiceRegistrations;
using Pustok.Business.ServiceRegistrations;

namespace Pustok.Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();



        //builder.Services.AddDbContext<AppDbContext>

        builder.Services.AddDataAccessServices(builder.Configuration);
        builder.Services.AddBusinessServices();


        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(); // Enables middleware to serve generated Swagger as a JSON endpoint
            app.UseSwaggerUI(); // Enables middleware to serve swagger-ui (HTML, JS, CSS, etc.)
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
