using TaskTracker.Application.Dtos;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Mappings;

public static class TaskMappings
{
    public static TaskResponse ToResponse(this TaskItem t) =>
        new(t.Id, t.ProjectId, t.Title, t.Description,
            t.Status, t.Priority, t.DueDateUtc, t.CreatedAtUtc);

    public static TaskItem ToEntity(this CreateTaskRequest r, int projectId, DateTime nowUtc) =>
        new()
        {
            ProjectId = projectId,
            Title = r.Title.Trim(),
            Description = r.Description,
            Priority = r.Priority,
            DueDateUtc = r.DueDateUtc,
            CreatedAtUtc = nowUtc
        };

    public static void ApplyTo(this UpdateTaskRequest r, TaskItem t)
    {
        t.Title = r.Title.Trim();
        t.Description = r.Description;
        t.Status = r.Status;
        t.Priority = r.Priority;
        t.DueDateUtc = r.DueDateUtc;
    }
}