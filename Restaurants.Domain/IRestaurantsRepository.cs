using Restaurants.Domain.Entities;

namespace Restaurants.Domain;

public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync();
}
