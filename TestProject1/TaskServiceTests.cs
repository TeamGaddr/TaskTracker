using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using TaskTracker.Application.Abstractions;
using TaskTracker.Application.Dtos;
using TaskTracker.Application.Exceptions;
using TaskTracker.Application.Services;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Tests;

public class TaskServiceTests
{
    private readonly ITaskRepository _tasks = Substitute.For<ITaskRepository>();
    private readonly IProjectRepository _projects = Substitute.For<IProjectRepository>();
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        _service = new TaskService(
            _tasks, _projects, TimeProvider.System, NullLogger<TaskService>.Instance);
    }

    [Fact]
    public async Task CreateAsync_WhenProjectDoesNotExist_ThrowsAndSavesNothing()
    {
        // Arrange: ??? make _projects.ExistsAsync(...) return false

        var request = new CreateTaskRequest("Write docs", null, TaskPriority.Medium, null);

        // Act + Assert: ??? ThrowsAsync<NotFoundException> around _service.CreateAsync(...)

        await _tasks.DidNotReceive().AddAsync(
            Arg.Any<TaskItem>(), Arg.Any<CancellationToken>());
    }
}