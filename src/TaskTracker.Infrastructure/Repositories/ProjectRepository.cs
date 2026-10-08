using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.Abstractions;
using TaskTracker.Application.Common;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Persistence;

namespace TaskTracker.Infrastructure.Repositories;

public class ProjectRepository(AppDbContext db) : IProjectRepository
{
    public Task<Project?> GetByIdAsync(int id, CancellationToken ct) =>
        db.Projects.FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<Project?> GetByIdWithTasksAsync(int id, CancellationToken ct) =>
        db.Projects
            .AsNoTracking()
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<PagedResult<Project>> ListAsync(
        string? nameContains, int page, int pageSize, CancellationToken ct)
    {
        var query = db.Projects.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(nameContains))
            query = query.Where(p => p.Name.Contains(nameContains));

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderBy(p => p.Name).ThenBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<Project>(items, page, pageSize, total);
    }

    public Task<bool> ExistsAsync(int id, CancellationToken ct) =>
        db.Projects.AnyAsync(p => p.Id == id, ct);

    public async Task AddAsync(Project project, CancellationToken ct)
    {
        db.Projects.Add(project);
        await db.SaveChangesAsync(ct);
    }

    public Task UpdateAsync(Project project, CancellationToken ct) =>
        db.SaveChangesAsync(ct); // entity is already tracked from GetByIdAsync

    public async Task DeleteAsync(Project project, CancellationToken ct)
    {
        db.Projects.Remove(project);
        await db.SaveChangesAsync(ct);
    }
}