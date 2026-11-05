using System.ComponentModel.DataAnnotations;

namespace AspNetAssignments.Models.Project;

public class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; } = false;

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
