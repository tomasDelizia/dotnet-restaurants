using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Repositories;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistance;
using Restaurants.Domain.Constants;
using System.Linq.Expressions;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
{
    // Columns selector dictionary to use in Linq expressions
    private static readonly Dictionary<string, Expression<Func<Restaurant, object>>> columnsSelector = new()
    {
        { nameof(Restaurant.Name), r => r.Name },
        { nameof(Restaurant.Description), r => r.Description },
        { nameof(Restaurant.Category), r => r.Category }
    };
    
    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        return await dbContext.Restaurants.ToListAsync();
    }

    public async Task<(IEnumerable<Restaurant> restaurants, int totalCount)> GetAllMatchingAsync(
        string? searchPhrase,
        int pageNumber,
        int pageSize,
        string? sortBy,
        SortDirection sortDirection)
    {
        var offset = pageSize * (pageNumber - 1);
        var searchPhraseLower = searchPhrase?.ToLower();

        var query = dbContext.Restaurants
            .Where(r => string.IsNullOrEmpty(searchPhraseLower)
                || r.Name.ToLower().Contains(searchPhraseLower)
                || r.Description.ToLower().Contains(searchPhraseLower));

        var totalCount = await query.CountAsync();

        if (sortBy != null)
        {
            var selectedColumn = columnsSelector[sortBy];
            query = sortDirection == SortDirection.Ascending ?
                query.OrderBy(selectedColumn) : query.OrderByDescending(selectedColumn);
        }

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
