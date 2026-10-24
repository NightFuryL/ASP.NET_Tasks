namespace Classwork_18_10_2026.DTO;
public class UserDTO
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public int Age { get; set; }
    public string? AvatarURL { get; set; }
    public DateTime CreatedAt { get; set; }
}
