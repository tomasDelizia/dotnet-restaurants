using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantRequestHandler(
    IRestaurantsRepository restaurantsRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService,
    ILogger<DeleteRestaurantRequestHandler> logger) : IRequestHandler<DeleteRestaurantCommand>
{
    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting restaurant with id {RequestId}", request.Id);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
        {
            throw new ForbiddenException(nameof(Restaurant), request.Id.ToString(), ResourceOperation.Delete);
        }
        await restaurantsRepository.DeleteAsync(restaurant);
    }
}
