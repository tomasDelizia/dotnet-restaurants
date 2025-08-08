using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;
using Restaurants.Domain.Entities;
using Restaurants.Application.Users;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler(
    IRestaurantsRepository restaurantsRepository,
    IUserContext userContext,
    ILogger<CreateRestaurantCommandHandler> logger,
    IMapper mapper) : IRequestHandler<CreateRestaurantCommand, Guid>
{
    public async Task<Guid> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Creating restaurant {@Restaurant} for owner {UserName} [{UserId}]",
            request,
            currentUser?.Email,
            currentUser?.Id);
        var restaurant = mapper.Map<Restaurant>(request);
        restaurant.OwnerId = currentUser!.Id;
        var id = await restaurantsRepository.CreateAsync(restaurant);
        return id;
    }
}
