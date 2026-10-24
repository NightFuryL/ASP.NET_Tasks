using Practice_11_10_2026.Models;

namespace Practice_11_10_2026.DTOs;

public class TaskItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; }
    public Practice_11_10_2026.Models.TaskStatus Status { get; set; }
    public Guid ProjectId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateTaskItemDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; }
    public Practice_11_10_2026.Models.TaskStatus Status { get; set; }
    public Guid ProjectId { get; set; }
}

public class UpdateTaskItemDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; }
    public Practice_11_10_2026.Models.TaskStatus Status { get; set; }
}