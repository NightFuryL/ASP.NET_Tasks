using FluentValidation;
using AspNetAssignments.DTOs.User;

namespace AspNetAssignments.Validators.User;

/// <summary>
/// Валідатор для перевірки коректності даних при створенні нового користувача
/// </summary>
public class CreateUserDtoValidator : AbstractValidator<CreateUserDTO>
{
    private static readonly string[] AllowedExtensions = { ".jpeg", ".jpg", ".png", ".webp" };
    private const long MaxFileSize = 2 * 1024 * 1024; // 2 MB

    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ім'я є обов'язковим.")
            .MaximumLength(100).WithMessage("Ім'я не повинно перевищувати 100 символів.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Прізвище є обов'язковим.")
            .MaximumLength(100).WithMessage("Прізвище не повинно перевищувати 100 символів.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email є обов'язковим.")
            .EmailAddress().WithMessage("Некоректний формат email.")
            .MaximumLength(255).WithMessage("Email не повинен перевищувати 255 символів.");

        RuleFor(x => x.Age)
            .InclusiveBetween(0, 150).WithMessage("Вік має бути в діапазоні від 0 до 150.");

        When(x => x.Avatar != null, () =>
        {
            RuleFor(x => x.Avatar!.FileName)
                .Must(fileName =>
                {
                    var ext = Path.GetExtension(fileName).ToLowerInvariant();
                    return AllowedExtensions.Contains(ext);
                })
                .WithMessage("Неприпустимий формат файлу. Дозволені формати: .jpg, .jpeg, .png, .webp");

            RuleFor(x => x.Avatar!.Length)
                .LessThanOrEqualTo(MaxFileSize)
                .WithMessage("Розмір аватара не повинен перевищувати 2 МБ.");
        });
    }
}
