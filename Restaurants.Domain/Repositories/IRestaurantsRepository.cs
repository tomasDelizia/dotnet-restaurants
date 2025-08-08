using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task<(IEnumerable<Restaurant> restaurants, int totalCount)> GetAllMatchingAsync(
        string? searchPhrase,
        int pageNumber,
        int pageSize,
        string? sortBy,
        SortDirection sortDirection);
    Task<IEnumerable<Restaurant>?> GetAllOfOwnerAsync(string ownerId);
    Task<Restaurant?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(Restaurant restaurant);
    Task DeleteAsync(Restaurant restaurant);
    Task SaveChangesAsync();
}
