using System.ComponentModel.DataAnnotations;
using AspNetAssignments.Models.Project;

namespace AspNetAssignments.DTOs.Project;

public class TaskItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; }
    public AspNetAssignments.Models.Project.TaskStatus Status { get; set; }
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

    public AspNetAssignments.Models.Project.TaskStatus Status { get; set; } = AspNetAssignments.Models.Project.TaskStatus.ToDo;

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

    public AspNetAssignments.Models.Project.TaskStatus Status { get; set; }
}
