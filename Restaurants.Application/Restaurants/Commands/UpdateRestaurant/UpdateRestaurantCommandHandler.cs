using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(
    IRestaurantsRepository restaurantsRepository,
    ILogger<UpdateRestaurantCommandHandler> logger,
    IMapper mapper) : IRequestHandler<UpdateRestaurantCommand, bool>
{
    public async Task<bool> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating restaurant of id {RequestId} with {@Request}", request.Id, request);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);
        if (restaurant == null) return false;
        // Gets all the properties of the request and add them to the entity.
        mapper.Map(request, restaurant);
        // Equivalent code:
        // restaurant.Name = request.Name;
        // restaurant.Description = request.Description;
        // restaurant.HasDelivery = request.HasDelivery;
        await restaurantsRepository.SaveChangesAsync();
        return true;
    }
}
