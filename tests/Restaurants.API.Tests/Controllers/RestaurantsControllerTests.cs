using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Seeders;

namespace Restaurants.API.Tests.Controllers;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();
    private readonly Mock<IRestaurantSeeder> _restaurantSeederMock = new();
    private readonly WebApplicationFactory<Program> _factory;

    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                // Autenticate and authorize all requests.
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                // Replace actual Database Repository and Seeder with mock.
                services
                    .Replace(ServiceDescriptor
                        .Scoped(typeof(IRestaurantsRepository), _ => _restaurantsRepositoryMock.Object))
                    .Replace(ServiceDescriptor
                        .Scoped(typeof(IRestaurantSeeder), _ => _restaurantSeederMock.Object));
            });
        });
    }

    [Fact]
    public async Task GetAll_ForValidRequest_ShouldReturn200OK()
    {
        // arrange
        var client = _factory.CreateClient();

        // act
        var result = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

        // assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAll_ForInvalidRequest_ShouldReturn400BadRequest()
    {
        // arrange
        var client = _factory.CreateClient();

        // act
        var result = await client.GetAsync("/api/restaurants");

        // assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetById_ForNonExistingId_ShouldReturn404NotFound()
    {
        // arrange
        var id = Guid.NewGuid();
        _restaurantsRepositoryMock
            .Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync((Restaurant?)null);
        var client = _factory.CreateClient();

        // act
        var result = await client.GetAsync($"/api/restaurants/{id}");

        // assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_ForExistingId_ShouldReturn200OK()
    {
        // arrange
        var id = Guid.NewGuid();
        var restaurant = new Restaurant
        {
            Id = id,
            Name = "Test",
            Description = "Test description"
        };
        _restaurantsRepositoryMock
            .Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(restaurant);
        var client = _factory.CreateClient();

        // act
        var result = await client.GetAsync($"/api/restaurants/{id}");
        var restaurantDto = await result.Content.ReadFromJsonAsync<RestaurantDto>();

        // assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        restaurantDto.Should().NotBeNull();
        restaurantDto.Name.Should().Be("Test");
        restaurantDto.Description.Should().Be("Test description");
    }
}
