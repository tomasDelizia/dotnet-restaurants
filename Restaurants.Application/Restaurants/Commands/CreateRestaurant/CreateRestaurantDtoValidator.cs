using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> validCategories = ["Italian", "Mexican", "Japanese", "American", "Indian"];

    public CreateRestaurantCommandValidator()
    {
        RuleFor(command => command.Name)
        .Length(3, 100);

        RuleFor(command => command.Description)
        .NotEmpty()
        .WithMessage("Description is required");

        RuleFor(command => command.Category)
        .Must(validCategories.Contains)
        .WithMessage("Please enter a valid category");
        // Equivalent code:
        // RuleFor(command => command.Category)
        // .Custom((value, context) =>
        // {
        //     var isValid = validCategories.Contains(value);
        //     if (!isValid) context.AddFailure("Category", "Please enter a valid category");
        // });

        RuleFor(command => command.ContactEmail)
        .EmailAddress()
        .WithMessage("Please enter a valid email");

        RuleFor(command => command.PostalCode)
        .Matches(@"^\d{2}-\d{3}$")
        .WithMessage("Please enter a valid postal code (XX-XXX)");
    }

}

