using Microsoft.Extensions.Logging;
using TaskTracker.Application.Abstractions;
using TaskTracker.Application.Common;
using TaskTracker.Application.Dtos;
using TaskTracker.Application.Exceptions;
using TaskTracker.Application.Mappings;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Services;

public class ProjectService(
    IProjectRepository projects,
    TimeProvider clock,
    ILogger<ProjectService> logger) : IProjectService
{
    public async Task<PagedResult<ProjectResponse>> ListAsync(
        string? nameContains, int page, int pageSize, CancellationToken ct)
    {
        var (p, size) = Paging.Normalize(page, pageSize);
        var result = await projects.ListAsync(nameContains, p, size, ct);
        return result.Map(x => x.ToResponse());
    }

    public async Task<ProjectDetailResponse> GetAsync(int id, CancellationToken ct)
    {
        var project = await projects.GetByIdWithTasksAsync(id, ct)
            ?? throw new NotFoundException(nameof(Project), id);
        return project.ToDetailResponse();
    }

    public async Task<ProjectResponse> CreateAsync(CreateProjectRequest request, CancellationToken ct)
    {
        var project = request.ToEntity(clock.GetUtcNow().UtcDateTime);
        await projects.AddAsync(project, ct);
        logger.LogInformation("Created project {ProjectId}", project.Id);
        return project.ToResponse();
    }

    public async Task UpdateAsync(int id, UpdateProjectRequest request, CancellationToken ct)
    {
        var project = await projects.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Project), id);
        request.ApplyTo(project);
        await projects.UpdateAsync(project, ct);
        logger.LogInformation("Updated project {ProjectId}", id);
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var project = await projects.GetByIdAsync(id, ct)
            ?? throw new NotFoundException(nameof(Project), id);
        await projects.DeleteAsync(project, ct);
        logger.LogInformation("Deleted project {ProjectId}", id);
    }
}