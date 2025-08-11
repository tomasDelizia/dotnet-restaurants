using FluentValidation;

namespace Restaurants.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
{
    public CreateDishCommandValidator()
    {
        RuleFor(command => command.Price)
        .GreaterThanOrEqualTo(0)
        .WithMessage("Price must be non-negative");

        RuleFor(command => command.KiloCalories)
        .GreaterThanOrEqualTo(0)
        .WithMessage("Price must be non-negative");

        RuleFor(command => command.Name)
        .Length(3, 100);

        RuleFor(command => command.Description)
        .NotEmpty()
        .WithMessage("Description is required");
    }
}
