using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
{
    public UpdateRestaurantCommandValidator()
    {
        RuleFor(command => command.Name)
        .Length(3, 100);

        RuleFor(command => command.Description)
        .NotEmpty()
        .WithMessage("Description is required");
    }

}
