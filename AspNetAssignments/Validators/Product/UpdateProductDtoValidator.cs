using FluentValidation;
using AspNetAssignments.DTOs.Product;

namespace AspNetAssignments.Validators.Product;

/// <summary>
/// Валідатор для перевірки коректності даних при оновленні наявного товару
/// </summary>
public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDTO>
{
    /// <summary>
    /// Ініціалізація правил валідації полів UpdateProductDTO
    /// </summary>
    public UpdateProductDtoValidator()
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
