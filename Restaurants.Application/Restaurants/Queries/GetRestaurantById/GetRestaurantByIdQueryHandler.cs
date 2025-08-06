using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQueryHandler(
    IRestaurantsRepository restaurantsRepository,
    ILogger<GetRestaurantByIdQueryHandler> logger,
    IMapper mapper) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto?>
{
    public async Task<RestaurantDto?> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting restaurant with id {}", request.Id);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);
        // return restaurant != null ? RestaurantDto.FromEntity(restaurant) : null;
        return mapper.Map<RestaurantDto?>(restaurant);
    }
}
