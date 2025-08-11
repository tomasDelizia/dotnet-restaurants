using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Authorization.Services;

public class RestaurantAuthorizationService(
    IUserContext userContext,
    ILogger<RestaurantAuthorizationService> logger
) : IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
    {
        // Check authorization logic for a given resource
        var user = userContext.GetCurrentUser();
        logger.LogInformation("Authorizing user {UserEmail} [{UserId}], to {Operation} on restaurant {RestaurantName}",
            user?.Email,
            user?.Id,
            resourceOperation,
            restaurant.Name);
        // Create/Read allowed by default
        if (resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
        {
            logger.LogInformation("Create/Read operation successfully authorized");
            return true;
        }
        // Delete allowed by admins
        if (resourceOperation == ResourceOperation.Delete && user!.HasRole("Admin"))
        {
            logger.LogInformation("Delete operation successfully authorized on Admin user");
            return true;
        }
        // Update/Delete allowed by owners
        if ((resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update)
            && user!.Id == restaurant.OwnerId)
        {
            logger.LogInformation("Update/Delete operation successfully authorized on restaurant owner");
            return true;
        }
        return false;
    }
}
