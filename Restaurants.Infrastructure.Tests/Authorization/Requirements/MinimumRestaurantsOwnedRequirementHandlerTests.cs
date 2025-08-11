using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.Requirements;
using Xunit;

namespace Restaurants.Infrastructure.Tests.Authorization.Requirements;

public class MinimumRestaurantsOwnedRequirementHandlerTests
{
    [Fact()]
    public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurats_ShouldSucceed()
    {
        // arrange
        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
        var userContextMock = new Mock<IUserContext>();
        userContextMock
            .Setup(u => u.GetCurrentUser())
            .Returns(currentUser);

        List<Restaurant> restaurants = [
            new() {
                OwnerId = currentUser.Id,
            },
             new() {
                OwnerId = currentUser.Id,
            }
        ];

        var repositoryMock = new Mock<IRestaurantsRepository>();
        repositoryMock
            .Setup(r => r.GetAllOfOwnerAsync(currentUser.Id))
            .ReturnsAsync(restaurants);

        var requirement = new MinimumRestaurantsOwnedRequirement(2);
        var handler = new MinimumRestaurantsOwnedRequirementHandler(
            userContextMock.Object,
            repositoryMock.Object,
            NullLogger<MinimumRestaurantsOwnedRequirementHandler>.Instance
        );
        var context = new AuthorizationHandlerContext([requirement], null, null);

        // act
        await handler.HandleAsync(context);

        // assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Fact()]
    public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurats_ShouldFail()
    {
        // arrange
        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
        var userContextMock = new Mock<IUserContext>();
        userContextMock
            .Setup(u => u.GetCurrentUser())
            .Returns(currentUser);

        List<Restaurant> restaurants = [
            new() {
                OwnerId = currentUser.Id,
            }
        ];

        var repositoryMock = new Mock<IRestaurantsRepository>();
        repositoryMock
            .Setup(r => r.GetAllOfOwnerAsync(currentUser.Id))
            .ReturnsAsync(restaurants);

        var requirement = new MinimumRestaurantsOwnedRequirement(2);
        var handler = new MinimumRestaurantsOwnedRequirementHandler(
            userContextMock.Object,
            repositoryMock.Object,
            NullLogger<MinimumRestaurantsOwnedRequirementHandler>.Instance
        );
        var context = new AuthorizationHandlerContext([requirement], null, null);

        // act
        await handler.HandleAsync(context);

        // assert
        context.HasFailed.Should().BeTrue();
    }
}
