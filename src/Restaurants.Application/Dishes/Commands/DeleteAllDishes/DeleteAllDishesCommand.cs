using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteAllDishes;

public class DeleteAllDishesCommand(Guid restaurantId) : IRequest
{
    public Guid RestaurantId { get; } = restaurantId;
}
