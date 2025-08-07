using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.UpdateUserDetails;

public class UpdateUserDetailsCommandHandler(
    IUserContext userContext,
    IUserStore<User> userStore,
    ILogger<UpdateUserDetailsCommandHandler> logger
) : IRequestHandler<UpdateUserDetailsCommand>
{
    public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();
        logger.LogInformation(
            "Updating user {UserId} with values {@Request}",
            user!.Id,
            request);
        var dbUser = await userStore.FindByIdAsync(user!.Id, cancellationToken) ?? throw new NotFoundException(nameof(User), user!.Id);
        dbUser.Nationality = request.Nationality;
        dbUser.DateOfBirth = request.DateOfBirth;
        await userStore.UpdateAsync(dbUser, cancellationToken);
    }
}
