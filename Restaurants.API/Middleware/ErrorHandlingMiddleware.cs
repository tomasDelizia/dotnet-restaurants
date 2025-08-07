using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Middleware;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException e)
        {
            logger.LogWarning("Resource not found: {Message}", e.Message);
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(e.Message);
        }
        catch (Exception e)
        {
            logger.LogError(e, "An unexpected error ocurred: {Message}", e.Message);
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong when handling the request");
        }
    }
}