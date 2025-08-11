using FluentAssertions;
using Xunit;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Tests.Users;

public class CurrentUserTests
{
    // TestMethod_Scenario_ExpectResult
    [Theory()]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void HasRole_WithMatchingRole_ShouldReturnTrue(string roleName)
    {
        // arrange
        var currentUser = new CurrentUser(
            "1",
            "test@test.com",
            [UserRoles.Admin, UserRoles.User],
            null,
            null);
        // act
        var hasRole = currentUser.HasRole(roleName);
        // assert
        hasRole.Should().BeTrue();
    }

    [Fact()]
    public void HasRole_WithNoMatchingRole_ShouldReturnFalse()
    {
        // arrange
        var currentUser = new CurrentUser(
            "1",
            "test@test.com",
            [UserRoles.Admin, UserRoles.User],
            null,
            null);
        // act
        var hasRole = currentUser.HasRole(UserRoles.Owner);
        // assert
        hasRole.Should().BeFalse();
    }

    [Fact()]
    public void HasRole_WithNoMatchingRoleCase_ShouldReturnFalse()
    {
        // arrange
        var currentUser = new CurrentUser(
            "1",
            "test@test.com",
            [UserRoles.Admin, UserRoles.User],
            null,
            null);
        // act
        var hasRole = currentUser.HasRole(UserRoles.Admin.ToLower());
        // assert
        hasRole.Should().BeFalse();
    }

}
