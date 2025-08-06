using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(
    IRestaurantsRepository restaurantsRepository,
    ILogger<RestaurantsService> logger,
    IMapper mapper) : IRestaurantsService
{
    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants()
    {
        logger.LogInformation("Getting all restaurants");
        var restaurants = await restaurantsRepository.GetAllAsync();
        // return restaurants.Select(RestaurantDto.FromEntity);
        return mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
    }

    public async Task<RestaurantDto?> GetRestaurantById(string id)
    {
        if (!Guid.TryParse(id, out var guidId))
        {
            logger.LogError("Invalid id: {}", id);
            return null;
        }
        logger.LogInformation("Getting restaurant with id {}", guidId);
        var restaurant = await restaurantsRepository.GetByIdAsync(guidId);
        // return restaurant != null ? RestaurantDto.FromEntity(restaurant) : null;
        return mapper.Map<RestaurantDto?>(restaurant);
    }
}
