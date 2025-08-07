using Restaurants.API.Middleware;
using Serilog;

namespace Restaurants.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        var config = builder.Configuration;
        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        // Add logging
        builder.Host.UseSerilog((context, cfg) =>
            // Retrieve from appsettings.Development.json
            cfg.ReadFrom.Configuration(config)
        );
        // Add error handling middleware
        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddScoped<TimeLoggingMiddleware>();
    }
}
