using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantRequestHandler(
    IRestaurantsRepository restaurantsRepository,
    ILogger<DeleteRestaurantRequestHandler> logger) : IRequestHandler<DeleteRestaurantCommand, bool>
{
    public async Task<bool> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting restaurant with id {RequestId}", request.Id);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);
        if (restaurant == null) return false;
        await restaurantsRepository.DeleteAsync(restaurant);
        return true;
    }
}
