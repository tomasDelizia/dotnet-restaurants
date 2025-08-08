using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Domain.Constants;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RestaurantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
    {
        var query = new GetAllRestaurantsQuery();
        var restaurants = await mediator.Send(query);
        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = PolicyNames.IsArgentinian)]
    public async Task<ActionResult<RestaurantDto>> GetById([FromRoute] Guid id)
    {
        var query = new GetRestaurantByIdQuery(id);
        var restaurant = await mediator.Send(query);
        return restaurant != null ? Ok(restaurant) : NotFound();
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Owner)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantCommand command)
    {
        Guid id = await mediator.Send(command);
        // The created restaurant can be found at the GetById location
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRestaurant([FromRoute] Guid id)
    {
        var command = new DeleteRestaurantCommand(id);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRestaurant(
        [FromRoute] Guid id,
        [FromBody] UpdateRestaurantCommand command)
    {
        command.Id = id;
        await mediator.Send(command);
        return NoContent();
    }
}
