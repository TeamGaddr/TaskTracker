using TaskTracker.Application.Common;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Abstractions;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(int id, CancellationToken ct);

    Task<PagedResult<TaskItem>> ListByProjectAsync(
        int projectId, TaskFilter filter, int page, int pageSize, CancellationToken ct);

    Task AddAsync(TaskItem task, CancellationToken ct);
    Task UpdateAsync(TaskItem task, CancellationToken ct);
    Task DeleteAsync(TaskItem task, CancellationToken ct);
}