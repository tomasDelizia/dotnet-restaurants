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

    public async Task<Restaurant?> GetByIdAsync(Guid id)
    {
        return await dbContext.Restaurants
        .Include(r => r.Dishes)
        .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Guid> CreateAsync(Restaurant restaurant)
    {
        dbContext.Restaurants.Add(restaurant);
        await dbContext.SaveChangesAsync();
        // Id will be set after saving to the DB.
        return restaurant.Id;
    }

    public async Task DeleteAsync(Restaurant restaurant)
    {
        dbContext.Restaurants.Remove(restaurant);
        await dbContext.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}
