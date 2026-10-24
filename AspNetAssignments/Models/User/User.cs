using System.ComponentModel.DataAnnotations;

namespace AspNetAssignments.Models.User;

/// <summary>
/// Сутність користувача системи з підтримкою завантаження аватара
/// </summary>
public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = "User";

    [Range(0, 150)]
    public int Age { get; set; }

    public string? AvatarURL { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; } = false;
}
