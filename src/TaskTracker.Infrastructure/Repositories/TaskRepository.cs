using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.Abstractions;
using TaskTracker.Application.Common;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Persistence;

namespace TaskTracker.Infrastructure.Repositories;

public class TaskRepository(AppDbContext db) : ITaskRepository
{
    public Task<TaskItem?> GetByIdAsync(int id, CancellationToken ct) =>
        db.Tasks.FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<PagedResult<TaskItem>> ListByProjectAsync(
        int projectId, TaskFilter filter, int page, int pageSize, CancellationToken ct)
    {
        var query = db.Tasks
            .AsNoTracking()
            .Where(t => t.ProjectId == projectId);

        if (filter.Status is not null)
            query = query.Where(t => t.Status == filter.Status);

        if (filter.Priority is not null)
            query = query.Where(t => t.Priority == filter.Priority);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(t => t.CreatedAtUtc).ThenBy(t => t.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<TaskItem>(items, page, pageSize, total);
    }

    public async Task AddAsync(TaskItem task, CancellationToken ct)
    {
        db.Tasks.Add(task);
        await db.SaveChangesAsync(ct);
    }

    public Task UpdateAsync(TaskItem task, CancellationToken ct) =>
        db.SaveChangesAsync(ct); // already tracked from GetByIdAsync

    public async Task DeleteAsync(TaskItem task, CancellationToken ct)
    {
        db.Tasks.Remove(task);
        await db.SaveChangesAsync(ct);
    }
}