namespace TaskTracker.Domain.Entities;

public class Project
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public ICollection<TaskItem> Tasks { get; set; } = [];
}