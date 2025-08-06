using AutoMapper;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Dtos;

public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    {
        CreateMap<Restaurant, RestaurantDto>()
        .ForMember(dest => dest.City, opt => opt.MapFrom(r => r.Address == null ? null : r.Address.City))
        .ForMember(dest => dest.Street, opt => opt.MapFrom(r => r.Address == null ? null : r.Address.Street))
        .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(r => r.Address == null ? null : r.Address.PostalCode))
        .ForMember(dest => dest.Dishes, opt => opt.MapFrom(r => r.Dishes));
    }

}
