using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(
    IRestaurantsRepository restaurantsRepository,
    ILogger<CreateRestaurantCommandHandler> logger,
    IMapper mapper) : IRequestHandler<CreateRestaurantCommand, Guid>
{
    public async Task<Guid> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating restaurant {@Restaurant}", request);
        var restaurant = mapper.Map<Restaurant>(request);
        var id = await restaurantsRepository.CreateAsync(restaurant);
        return id;
    }
}
