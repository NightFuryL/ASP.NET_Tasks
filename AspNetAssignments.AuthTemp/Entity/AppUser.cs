using Microsoft.AspNetCore.Identity;
namespace AspNetAssignments.AuthTemp.Entity;

public class AppUser : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public DateTime RegistrationAt { get; set; } = DateTime.UtcNow;
}
