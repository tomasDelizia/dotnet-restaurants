using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.DeleteAllDishes;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Queries.GetDishById;
using Restaurants.Application.Dishes.Queries.GetDishes;

namespace Restaurants.API.Controllers;

[Route("api/restaurants/{restaurantId}/[controller]")]
[ApiController]
public class DishesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateDish([FromRoute] Guid restaurantId, CreateDishCommand command)
    {
        command.RestaurantId = restaurantId;
        await mediator.Send(command);
        return Created();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetDishes([FromRoute] Guid restaurantId)
    {
        var query = new GetDishesQuery(restaurantId);
        var dishes = await mediator.Send(query);
        return Ok(dishes);
    }

    [HttpGet("{dishId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DishDto>> GetDish(
        [FromRoute] Guid restaurantId,
        [FromRoute] int dishId)
    {
        var query = new GetDishByIdQuery(restaurantId, dishId);
        var dish = await mediator.Send(query);
        return Ok(dish);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAllDishes([FromRoute] Guid restaurantId)
    {
        var command = new DeleteAllDishesCommand(restaurantId);
        await mediator.Send(command);
        return NoContent();
    }
}
