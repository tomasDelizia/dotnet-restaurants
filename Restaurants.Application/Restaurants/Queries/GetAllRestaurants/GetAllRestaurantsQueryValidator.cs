using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private static readonly int[] allowedPageSizes = [5, 10, 15, 30];
    private static readonly string[] allowedSortOptions = [
        nameof(RestaurantDto.Name),
        nameof(RestaurantDto.Description),
        nameof(RestaurantDto.Category)
    ];

    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(r => r.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(r => r.PageSize)
            .Must(value => allowedPageSizes.Contains(value))
            .WithMessage($"Page size must be either {string.Join(", ", allowedPageSizes)}");

        RuleFor(r => r.SortBy)
            .Must(value => allowedSortOptions.Contains(value))
            .When(r => r.SortBy != null)
            .WithMessage($"Sort by must be either {string.Join(", ", allowedSortOptions)}");
    }
}
