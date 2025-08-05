using Microsoft.Extensions.Logging;
using Restaurants.Domain;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(IRestaurantsRepository restaurantsRepository, ILogger<RestaurantsService> logger) : IRestaurantsService
{
    public async Task<IEnumerable<Restaurant>> GetAllRestaurants()
    {
        logger.LogInformation("Getting all restaurants");
        return await restaurantsRepository.GetAllAsync();
    }
}
