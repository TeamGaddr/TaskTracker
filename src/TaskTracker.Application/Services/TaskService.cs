using Microsoft.Extensions.Logging;
using TaskTracker.Application.Abstractions;
using TaskTracker.Application.Common;
using TaskTracker.Application.Dtos;
using TaskTracker.Application.Exceptions;
using TaskTracker.Application.Mappings;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Services;

public class TaskService(
    ITaskRepository tasks,
    IProjectRepository projects,
    TimeProvider clock,
    ILogger<TaskService> logger) : ITaskService
{
    public async Task<PagedResult<TaskResponse>> ListByProjectAsync(
        int projectId, TaskFilter filter, int page, int pageSize, CancellationToken ct)
    {
        if (!await projects.ExistsAsync(projectId, ct))
            throw new NotFoundException(nameof(Project), projectId);

        var (p, size) = Paging.Normalize(page, pageSize);
        var result = await tasks.ListByProjectAsync(projectId, filter, p, size, ct);
        return result.Map(t => t.ToResponse());
    }

    public async Task<TaskResponse> GetAsync(int id, CancellationToken ct)
    {
        var task = await tasks.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(TaskItem), id);
        return task.ToResponse();
    }

    public async Task<TaskResponse> CreateAsync(
        int projectId, CreateTaskRequest request, CancellationToken ct)
    {
        if (!await projects.ExistsAsync(projectId, ct))
            throw new NotFoundException(nameof(Project), projectId);

        var task = request.ToEntity(projectId, clock.GetUtcNow().UtcDateTime);
        await tasks.AddAsync(task, ct);
        logger.LogInformation("Created task {TaskId} in project {ProjectId}", task.Id, projectId);
        return task.ToResponse();
    }

    public async Task UpdateAsync(int id, UpdateTaskRequest request, CancellationToken ct)
    {
        var task = await tasks.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(TaskItem), id);
        request.ApplyTo(task);
        await tasks.UpdateAsync(task, ct);
        logger.LogInformation("Updated task {TaskId}", id);
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var task = await tasks.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(TaskItem), id);
        await tasks.DeleteAsync(task, ct);
        logger.LogInformation("Deleted task {TaskId}", id);
    }
}