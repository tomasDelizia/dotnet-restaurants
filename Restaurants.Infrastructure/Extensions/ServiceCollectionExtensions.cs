using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Authorization.Requirements;
using Restaurants.Infrastructure.Persistance;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("RestaurantsDb");
        // By default, DbContext is added as a scoped dependency.
        services.AddDbContext<RestaurantsDbContext>(options => options
            .UseSqlServer(connectionString, options => options.EnableRetryOnFailure())
            .EnableSensitiveDataLogging()
        );

        // Add seeders
        services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();

        // Add repositories
        services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
        services.AddScoped<IDishesRepository, DishesRepository>();

        // Add identity services
        services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
            .AddEntityFrameworkStores<RestaurantsDbContext>();

        services.AddAuthorizationBuilder()
            .AddPolicy(PolicyNames.HasNationality, builder => builder.RequireClaim(AppClaimTypes.Nationality))
            .AddPolicy(PolicyNames.IsArgentinian, builder => builder.RequireClaim(AppClaimTypes.Nationality, "Argentinian"))
            .AddPolicy(PolicyNames.IsAdult, builder => builder.AddRequirements(
                new MinimumAgeRequirement(18)
            ));
        // Add custom requirments
        services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
    }
}
