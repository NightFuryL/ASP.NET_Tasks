using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AspNetAssignments.DTOs.User;

public class UpdateUserDTO : IValidatableObject
{
    [Required(ErrorMessage = "Ім'я є обов'язковим.")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Прізвище є обов'язковим.")]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email є обов'язковим.")]
    [EmailAddress(ErrorMessage = "Некоректний формат email.")]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = "User";

    [Range(0, 150, ErrorMessage = "Вік має бути в діапазоні від 0 до 150.")]
    public int Age { get; set; }

    public IFormFile? Avatar { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Avatar != null)
        {
            var allowedExtensions = new[] { ".jpeg", ".jpg", ".png", ".webp" };
            var extension = Path.GetExtension(Avatar.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                yield return new ValidationResult(
                    "Неприпустимий формат файлу. Дозволені формати: .jpg, .jpeg, .png, .webp",
                    new[] { nameof(Avatar) }
                );
            }

            const long maxFileSize = 2 * 1024 * 1024;
            if (Avatar.Length > maxFileSize)
            {
                yield return new ValidationResult(
                    "Розмір аватара не повинен перевищувати 2 МБ.",
                    new[] { nameof(Avatar) }
                );
            }
        }
    }
}
