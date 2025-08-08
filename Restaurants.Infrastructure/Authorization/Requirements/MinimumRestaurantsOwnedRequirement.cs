using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MinimumRestaurantsOwnedRequirement(int minAmount) : IAuthorizationRequirement
{
    public int MinAmount { get; } = minAmount;
 }
