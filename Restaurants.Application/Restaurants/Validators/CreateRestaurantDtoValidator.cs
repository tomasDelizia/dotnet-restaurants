using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Validators;

public class CreateRestaurantDtoValidator : AbstractValidator<CreateRestaurantDto>
{
    private readonly List<string> validCategories = ["Italian", "Mexican", "Japanese", "American", "Indian"];

    public CreateRestaurantDtoValidator()
    {
        RuleFor(dto => dto.Name)
        .Length(3, 100);

        RuleFor(dto => dto.Description)
        .NotEmpty()
        .WithMessage("Description is required");

        RuleFor(dto => dto.Category)
        .Must(validCategories.Contains)
        .WithMessage("Please enter a valid category");
        // Equivalent code:
        // RuleFor(dto => dto.Category)
        // .Custom((value, context) =>
        // {
        //     var isValid = validCategories.Contains(value);
        //     if (!isValid) context.AddFailure("Category", "Please enter a valid category");
        // });

        RuleFor(dto => dto.ContactEmail)
        .EmailAddress()
        .WithMessage("Please enter a valid email");

        RuleFor(dto => dto.PostalCode)
        .Matches(@"^\d{2}-\d{3}$")
        .WithMessage("Please enter a valid postal code (XX-XXX)");
    }

}

