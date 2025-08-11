using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandlerTests
{
    [Fact()]
    public async Task Handle_ForValidCommand_ReturnsCreatedRestaurantId()
    {
        // arrange
        var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();

        var mapperMock = new Mock<IMapper>();
        var command = new CreateRestaurantCommand();
        var restaurant = new Restaurant();
        // No need to test the mapping since it's part of other test.s
        mapperMock
            .Setup(m => m.Map<Restaurant>(command))
            .Returns(restaurant);

        var repositoryMock = new Mock<IRestaurantsRepository>();
        // Setup the CreateAsync method so that it ascynchronously returns a desired Guid.
        var guid = Guid.NewGuid();
        repositoryMock
            .Setup(repo => repo.CreateAsync(It.IsAny<Restaurant>()))
            .ReturnsAsync(guid);

        var userContextMock = new Mock<IUserContext>();
        var currentUser = new CurrentUser("owner-id", "owner@test.com", [], null, null);
        userContextMock
            .Setup(u => u.GetCurrentUser())
            .Returns(currentUser);

        var commandHandler = new CreateRestaurantCommandHandler(
            repositoryMock.Object,
            userContextMock.Object,
            loggerMock.Object,
            mapperMock.Object);

        // act
        var result = await commandHandler.Handle(command, CancellationToken.None);

        // assert
        result.Should().Be(guid);
        restaurant.OwnerId.Should().Be("owner-id");
        // Verify that the CreateAsync method was called only once.
        repositoryMock.Verify(r => r.CreateAsync(restaurant), Times.Once);
    }
}
