using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain;
using Restaurants.Domain.Entities;

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

    public async Task<RestaurantDto?> GetRestaurantById(Guid id)
    {
        logger.LogInformation("Getting restaurant with id {}", id);
        var restaurant = await restaurantsRepository.GetByIdAsync(id);
        // return restaurant != null ? RestaurantDto.FromEntity(restaurant) : null;
        return mapper.Map<RestaurantDto?>(restaurant);
    }

    public async Task<Guid> CreateRestaurant(CreateRestaurantDto createRestaurantDto)
    {
        logger.LogInformation("Creating restaurant {}", createRestaurantDto.Name);
        var restaurant = mapper.Map<Restaurant>(createRestaurantDto);
        var id = await restaurantsRepository.CreateAsync(restaurant);
        return id;
    }
}
