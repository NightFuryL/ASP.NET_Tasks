using System.ComponentModel.DataAnnotations;

namespace Practice_11_10_2026.Models;

public class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; } = false;

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}