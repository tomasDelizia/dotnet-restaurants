using Restaurants.Domain.Entities;

namespace Restaurants.Domain;

public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task<Restaurant?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(Restaurant restaurant);
    Task DeleteAsync(Restaurant restaurant);
}
