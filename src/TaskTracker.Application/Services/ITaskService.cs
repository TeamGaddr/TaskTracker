using TaskTracker.Application.Common;
using TaskTracker.Application.Dtos;

namespace TaskTracker.Application.Services;

public interface ITaskService
{
    Task<PagedResult<TaskResponse>> ListByProjectAsync(
        int projectId, TaskFilter filter, int page, int pageSize, CancellationToken ct);
    Task<TaskResponse> GetAsync(int id, CancellationToken ct);
    Task<TaskResponse> CreateAsync(int projectId, CreateTaskRequest request, CancellationToken ct);
    Task UpdateAsync(int id, UpdateTaskRequest request, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}