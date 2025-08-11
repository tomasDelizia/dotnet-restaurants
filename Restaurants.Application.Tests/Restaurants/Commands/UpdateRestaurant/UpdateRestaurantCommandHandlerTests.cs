using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantsRepository> _repositoryMock;
    private readonly Mock<IRestaurantAuthorizationService> _authorizationServiceMock;
    private readonly UpdateRestaurantCommandHandler _handler;

    public UpdateRestaurantCommandHandlerTests()
    {
        var loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _mapperMock = new Mock<IMapper>();
        _repositoryMock = new Mock<IRestaurantsRepository>();
        _authorizationServiceMock = new Mock<IRestaurantAuthorizationService>();
        _handler = new UpdateRestaurantCommandHandler(
            _repositoryMock.Object,
            _authorizationServiceMock.Object,
            loggerMock.Object,
            _mapperMock.Object);
    }
    
    [Fact]
    public async Task Handle_ForValidCommand_UpdatesRestaurant()
    {
        // arrange
        var id = Guid.NewGuid();
        var command = new UpdateRestaurantCommand
        {
            Id = id,
        };
        var restaurant = new Restaurant();

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(restaurant);
        _repositoryMock.Setup(repo => repo.SaveChangesAsync());

        _authorizationServiceMock
            .Setup(a => a.Authorize(restaurant, ResourceOperation.Update))
            .Returns(true);

        // act
        await _handler.Handle(command, CancellationToken.None);

        // assert
        // Verify that the GetById and SaveChanges method were called only once.
        _repositoryMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        _mapperMock.Verify(m => m.Map(command, restaurant), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ForNotFoundRestaurant_ShouldNotUpdateRestaurant()
    {
        // arrange
        var id = Guid.NewGuid();
        var command = new UpdateRestaurantCommand
        {
            Id = id,
        };

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync((Restaurant?)null);

        // act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // assert
        // Verify that the GetById and SaveChanges method were called only once.
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"{nameof(Restaurant)} with id {id} not found");
    }

    [Fact]
    public async Task Handle_ForUnauthorizedUser_ShouldNotUpdateRestaurant()
    {
        // arrange
        var id = Guid.NewGuid();
        var command = new UpdateRestaurantCommand
        {
            Id = id,
        };
        var restaurant = new Restaurant();

        _repositoryMock
            .Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(restaurant);

        _authorizationServiceMock
            .Setup(a => a.Authorize(restaurant, ResourceOperation.Update))
            .Returns(false);

        // act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // assert
        // Verify that the GetById and SaveChanges method were called only once.
        await act.Should().ThrowAsync<ForbiddenException>()
            .WithMessage($"{ResourceOperation.Update} operation on {nameof(Restaurant)} with id {id} not allowed");
    }
}
