using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Application.Dtos;

public record CreateProjectRequest(
    [Required, StringLength(200, MinimumLength = 1)] string Name,
    [StringLength(2000)] string? Description);

public record UpdateProjectRequest(
    [Required, StringLength(200, MinimumLength = 1)] string Name,
    [StringLength(2000)] string? Description);

public record ProjectResponse(int Id, string Name, string? Description, DateTime CreatedAtUtc);

public record ProjectDetailResponse(
    int Id, string Name, string? Description, DateTime CreatedAtUtc,
    IReadOnlyList<TaskResponse> Tasks);