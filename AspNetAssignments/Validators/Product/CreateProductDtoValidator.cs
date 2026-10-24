using FluentValidation;
using AspNetAssignments.DTOs.Product;

namespace AspNetAssignments.Validators.Product;

/// <summary>
/// Валідатор для перевірки коректності даних при створенні нового товару
/// </summary>
public class CreateProductDtoValidator : AbstractValidator<CreateProductDTO>
{
    /// <summary>
    /// Ініціалізація правил валідації полів CreateProductDTO
    /// </summary>
    public CreateProductDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.");
    }
}
