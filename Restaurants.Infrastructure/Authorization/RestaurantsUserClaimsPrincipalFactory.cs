using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Authorization;

public class RestaurantsUserClaimsPrincipalFactory(
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    IOptions<IdentityOptions> options
) : UserClaimsPrincipalFactory<User, IdentityRole> (userManager, roleManager, options)
{
    public override async Task<ClaimsPrincipal> CreateAsync(User user)
    {
        // Extend claims to include Nationality and DateOfBirth
        var id = await GenerateClaimsAsync(user);
        if (user.Nationality != null)
        {
            id.AddClaim(new Claim("Nationality", user.Nationality));
        }
        if (user.DateOfBirth != null)
        {
            id.AddClaim(new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyy-MM-dd")));
        }
        return new ClaimsPrincipal(id);
    }
}
