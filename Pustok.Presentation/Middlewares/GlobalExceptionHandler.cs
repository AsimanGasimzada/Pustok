using Pustok.Business.Abstractions;
using Pustok.Business.Dtos;

namespace Pustok.Presentation.Middlewares;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {

        try
        {
            await _next(context);

        }
        catch (Exception ex)
        {
            ResultDto errorResult = new()
            {
                IsSucced = false,
                Message = "Internal Server Error",
                StatusCode = 500
            };

            if (ex is IBaseException baseException)
            {
                errorResult.Message = ex.Message;
                errorResult.StatusCode = baseException.StatusCode;
            }


            context.Response.StatusCode = errorResult.StatusCode;

            await context.Response.WriteAsJsonAsync(errorResult);
            //await context.Response.Redirect("Home/Error"); //for mvc pages
        }
    }
}
