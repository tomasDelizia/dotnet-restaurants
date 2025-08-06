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

    public async Task<Restaurant?> GetRestaurantById(string id)
    {
        if (!Guid.TryParse(id, out var guidId))
        {
            logger.LogError("Invalid id: {}", id);
            return null;
        }
        logger.LogInformation("Getting restaurant with id {}", guidId);
        return await restaurantsRepository.GetByIdAsync(guidId);
    }
}
