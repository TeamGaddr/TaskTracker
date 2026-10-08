using TaskTracker.Application.Common;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Abstractions;

public interface IProjectRepository
{
    // Tracked: used when the service will modify the entity
    Task<Project?> GetByIdAsync(int id, CancellationToken ct);

    // Read-only, includes the tasks
    Task<Project?> GetByIdWithTasksAsync(int id, CancellationToken ct);

    Task<PagedResult<Project>> ListAsync(string? nameContains, int page, int pageSize, CancellationToken ct);
    Task<bool> ExistsAsync(int id, CancellationToken ct);

    Task AddAsync(Project project, CancellationToken ct);
    Task UpdateAsync(Project project, CancellationToken ct);
    Task DeleteAsync(Project project, CancellationToken ct);
}