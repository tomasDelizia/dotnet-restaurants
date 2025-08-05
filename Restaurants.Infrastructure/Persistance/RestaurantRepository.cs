using Microsoft.EntityFrameworkCore;
using Restaurants.Domain;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Persistance;

internal class RestaurantRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
{
    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        return await dbContext.Restaurants.ToListAsync();
    }
}
