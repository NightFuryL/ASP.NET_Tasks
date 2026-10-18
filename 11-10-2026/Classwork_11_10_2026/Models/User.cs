namespace Classwork_11_10_2026.Models;
/*
 postgresql query to fill the table for this model:
 INSERT INTO "Users" ("Id", "Email", "FirstName", "LastName", "Role", "CreatedAt", "IsDeleted")
 VALUES
   ('1', 'john.doe@example.com', 'John', 'Doe', 'User', '2023-01-01 00:00:00', false),
   ('2', 'jane.smith@example.com', 'Jane', 'Smith', 'Admin', '2023-01-01 00:00:00', false);
    
 */
public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
