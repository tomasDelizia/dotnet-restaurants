using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Tests.Users;

public class UserContextTests
{
    [Fact()]
    public void GetCurrentUserTest_WithAuthenticatedUser_ShouldReturnCurrentUser()
    {
        // arrange
        var httpContextAccesorMock = new Mock<IHttpContextAccessor>();
        var dateOfBirth = new DateOnly(2000, 1, 1);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Email, "test@test.com"),
            new(ClaimTypes.Role, UserRoles.Admin),
            new(ClaimTypes.Role, UserRoles.User),
            new("Nationality", "Spanish"),
            new("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd")),
        };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

        // Setup mock
        httpContextAccesorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
        {
            User = user
        });

        var userContext = new UserContext(httpContextAccesorMock.Object);

        // act
        var currentUser = userContext.GetCurrentUser();

        // asset
        currentUser.Should().NotBeNull();
        currentUser.Id.Should().Be("1");
        currentUser.Email.Should().Be("test@test.com");
        currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
        currentUser.Nationality.Should().Be("Spanish");
        currentUser.DateOfBirth.Should().Be(dateOfBirth);
    }

    [Fact()]
    public void GetCurrentUserTest_WithUserContextNotpreset_ShouldThrowInvalidOperationException()
    {
        // arrange
        var httpContextAccesorMock = new Mock<IHttpContextAccessor>();
        httpContextAccesorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);
        var userContext = new UserContext(httpContextAccesorMock.Object);

        // act
        Action action = () => userContext.GetCurrentUser();

        // asset
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("User context is not present");
    }
}
