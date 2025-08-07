using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteAllDishes;

public class DeleteAllDishesCommandHandler(
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository,
    ILogger<DeleteAllDishesCommand> logger
) : IRequestHandler<DeleteAllDishesCommand>
{
    public async Task Handle(DeleteAllDishesCommand request, CancellationToken cancellationToken)
    {
        logger.LogWarning("Deleting all dishes for restaurant with id {RestaurantId}", request.RestaurantId);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId) ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        await dishesRepository.DeleteAsync(restaurant.Dishes);
    }
}
