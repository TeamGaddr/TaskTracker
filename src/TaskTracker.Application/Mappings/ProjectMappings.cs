using TaskTracker.Application.Common;
using TaskTracker.Application.Dtos;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Mappings;

public static class ProjectMappings
{
    public static ProjectResponse ToResponse(this Project p) =>
        new(p.Id, p.Name, p.Description, p.CreatedAtUtc);

    public static ProjectDetailResponse ToDetailResponse(this Project p) =>
        new(p.Id, p.Name, p.Description, p.CreatedAtUtc,
            p.Tasks.Select(t => t.ToResponse()).ToList());

    public static Project ToEntity(this CreateProjectRequest r, DateTime nowUtc) =>
        new() { Name = r.Name.Trim(), Description = r.Description, CreatedAtUtc = nowUtc };

    public static void ApplyTo(this UpdateProjectRequest r, Project p)
    {
        p.Name = r.Name.Trim();
        p.Description = r.Description;
    }

    public static PagedResult<TOut> Map<TIn, TOut>(
        this PagedResult<TIn> page, Func<TIn, TOut> map) =>
        new(page.Items.Select(map).ToList(), page.Page, page.PageSize, page.TotalCount);
}