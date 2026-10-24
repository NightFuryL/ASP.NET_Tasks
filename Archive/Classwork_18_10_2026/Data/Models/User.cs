namespace Classwork_18_10_2026.Data.Models;
public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public int Age { get; set; }
    public string? AvatarURL { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
