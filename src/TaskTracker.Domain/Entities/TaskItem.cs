using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities;

public class TaskItem
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Todo;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime? DueDateUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
}