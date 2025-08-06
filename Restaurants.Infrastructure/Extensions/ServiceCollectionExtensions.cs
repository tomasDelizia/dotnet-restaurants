using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain;
using Restaurants.Infrastructure.Persistance;
using Restaurants.Infrastructure.Seeders;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("RestaurantsDb");
        // By default, DbContext is added as a scoped dependency.
        services.AddDbContext<RestaurantsDbContext>(options => options
            .UseSqlServer(connectionString)
            .EnableSensitiveDataLogging()
        );

        // Add seeders
        services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();

        // Add repositories
        services.AddScoped<IRestaurantsRepository, RestaurantRepository>();
    }
}
