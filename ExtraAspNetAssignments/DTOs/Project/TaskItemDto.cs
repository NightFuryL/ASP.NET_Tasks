using System.ComponentModel.DataAnnotations;
using ExtraAspNetAssignments.Models.Project;

namespace ExtraAspNetAssignments.DTOs.Project;

public class TaskItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; }
    public ExtraAspNetAssignments.Models.Project.TaskStatus Status { get; set; }
    public Guid ProjectId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateTaskItemDto
{
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public ExtraAspNetAssignments.Models.Project.TaskStatus Status { get; set; } = ExtraAspNetAssignments.Models.Project.TaskStatus.ToDo;

    [Required]
    public Guid ProjectId { get; set; }
}

public class UpdateTaskItemDto
{
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public TaskPriority Priority { get; set; }

    public ExtraAspNetAssignments.Models.Project.TaskStatus Status { get; set; }
}
