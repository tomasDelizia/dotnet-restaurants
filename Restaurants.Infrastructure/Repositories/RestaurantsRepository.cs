using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Repositories;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistance;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
{
    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        return await dbContext.Restaurants.ToListAsync();
    }

    public async Task<(IEnumerable<Restaurant> restaurants, int totalCount)> GetAllMatchingAsync(
        string? searchPhrase,
        int pageNumber,
        int pageSize)
    {
        var offset = pageSize * (pageNumber - 1);
        var searchPhraseLower = searchPhrase?.ToLower();

        var query = dbContext.Restaurants
            .Where(r => string.IsNullOrEmpty(searchPhraseLower)
                || r.Name.ToLower().Contains(searchPhraseLower)
                || r.Description.ToLower().Contains(searchPhraseLower));

        var totalCount = await query.CountAsync();

        var restaurants = await query
            .Skip(offset)
            .Take(pageSize)
            .ToListAsync();

        return (restaurants, totalCount);
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

    public async Task<IEnumerable<Restaurant>?> GetAllOfOwnerAsync(string ownerId)
    {
        var owner = await dbContext.Users
            .Include(u => u.OwnedRestaurants)
            .FirstOrDefaultAsync(u => u.Id == ownerId);
        return owner?.OwnedRestaurants;
    }
}
