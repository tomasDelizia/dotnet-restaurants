
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MinimumRestaurantsOwnedRequirementHandler(
    IUserContext userContext,
    IRestaurantsRepository restaurantsRepository,
    ILogger<MinimumRestaurantsOwnedRequirementHandler> logger
) : AuthorizationHandler<MinimumRestaurantsOwnedRequirement>
{
    protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumRestaurantsOwnedRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("User: {Email} - Handling MinimumRestaurantsOwnedRequirement",
        currentUser?.Email);

        if (currentUser == null)
        {
            logger.LogWarning("User is null");
            context.Fail();
            await Task.CompletedTask;
            return;
        }

        var restaurants = await restaurantsRepository.GetAllOfOwnerAsync(currentUser.Id);

        if (restaurants == null || restaurants!.Count() < requirement.MinAmount)
        {
            logger.LogWarning("Invalid amount of restaurants owned by user");
            context.Fail();
            await Task.CompletedTask;
            return;
        }
        logger.LogInformation("Authorization succeded");
        context.Succeed(requirement);
        await Task.CompletedTask;
    }
}
