using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;

namespace Restaurants.Infrastructure.Authorization.Requirements;

internal class MinimumAgeRequirementHandler(
    IUserContext userContext,
    ILogger<MinimumAgeRequirementHandler> logger
    ) : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("User: {Email}, date of birth {DoB} - Handling MinimumAgeRequirement",
        currentUser?.Email,
        currentUser?.DateOfBirth);

        var today = DateOnly.FromDateTime(DateTime.Today);
        if (currentUser?.DateOfBirth == null)
        {
            logger.LogWarning("User date of birth is null");
            context.Fail();
            return Task.CompletedTask;
        }
        bool isOverMinimumAge = currentUser?.DateOfBirth.Value.AddYears(requirement.MinimumAge) <= today;
        if (isOverMinimumAge)
        {
            logger.LogInformation("Authorization succeded");
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        logger.LogWarning("User is not of age");
        context.Fail();
        return Task.CompletedTask;
    }
}
