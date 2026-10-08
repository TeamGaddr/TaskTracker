using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Dtos;

public record CreateTaskRequest(
    [Required, StringLength(200, MinimumLength = 1)] string Title,
    [StringLength(2000)] string? Description,
    TaskPriority Priority,
    DateTime? DueDateUtc);

public record UpdateTaskRequest(
    [Required, StringLength(200, MinimumLength = 1)] string Title,
    [StringLength(2000)] string? Description,
    TaskItemStatus Status,
    TaskPriority Priority,
    DateTime? DueDateUtc);

public record TaskResponse(
    int Id, int ProjectId, string Title, string? Description,
    TaskItemStatus Status, TaskPriority Priority,
    DateTime? DueDateUtc, DateTime CreatedAtUtc);