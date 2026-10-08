using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.Common;
using TaskTracker.Application.Dtos;
using TaskTracker.Application.Services;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Api.Controllers;

[ApiController]
[Route("api")]
public class TasksController(ITaskService tasks) : ControllerBase
{
    [HttpGet("projects/{projectId:int}/tasks")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PagedResult<TaskResponse>>> ListByProject(
        int projectId,
        [FromQuery] TaskItemStatus? status,
        [FromQuery] TaskPriority? priority,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
        => Ok(await tasks.ListByProjectAsync(
            projectId, new TaskFilter(status, priority), page, pageSize, ct));

    [HttpPost("projects/{projectId:int}/tasks")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponse>> Create(
        int projectId, CreateTaskRequest request, CancellationToken ct)
    {
        var created = await tasks.CreateAsync(projectId, request, ct);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpGet("tasks/{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponse>> Get(int id, CancellationToken ct)
        => Ok(await tasks.GetAsync(id, ct));

    [HttpPut("tasks/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        int id, UpdateTaskRequest request, CancellationToken ct)
    {
        await tasks.UpdateAsync(id, request, ct);
        return NoContent();
    }

    [HttpDelete("tasks/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await tasks.DeleteAsync(id, ct);
        return NoContent();
    }
}