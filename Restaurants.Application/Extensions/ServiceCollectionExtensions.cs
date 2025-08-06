using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Restaurants.Application.Restaurants;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var appAssembly = typeof(ServiceCollectionExtensions).Assembly;
        var licenseKey = configuration["LuckyPennySoftware:LicenseKey"];

        // Add Services
        // services.AddScoped<IRestaurantsService, RestaurantsService>();
        services.AddMediatR(cfg =>
        {
            cfg.LicenseKey = licenseKey;
            cfg.RegisterServicesFromAssemblies(appAssembly);
        });

        // Add mapper
        services.AddAutoMapper(cfg =>
        {
            cfg.LicenseKey = licenseKey;
        }, appAssembly);

        // Add validators
        services
        .AddValidatorsFromAssembly(appAssembly)
        .AddFluentValidationAutoValidation();
    }
}
