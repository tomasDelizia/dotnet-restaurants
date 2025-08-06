using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Restaurants.Application.Restaurants;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Services
        services.AddScoped<IRestaurantsService, RestaurantsService>();

        var licenseKey = configuration["LuckyPennySoftware:LicenseKey"];
        // Add mapper
        services.AddAutoMapper(cfg =>
        {
            cfg.LicenseKey = licenseKey;
        }, typeof(ServiceCollectionExtensions).Assembly);
    }
}
