using TaskTracker.Application.Common;
using TaskTracker.Application.Dtos;

namespace TaskTracker.Application.Services;

public interface IProjectService
{
    Task<PagedResult<ProjectResponse>> ListAsync(string? nameContains, int page, int pageSize, CancellationToken ct);
    Task<ProjectDetailResponse> GetAsync(int id, CancellationToken ct);
    Task<ProjectResponse> CreateAsync(CreateProjectRequest request, CancellationToken ct);
    Task UpdateAsync(int id, UpdateProjectRequest request, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}