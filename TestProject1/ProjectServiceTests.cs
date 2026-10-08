using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using TaskTracker.Application.Abstractions;
using TaskTracker.Application.Dtos;
using TaskTracker.Application.Exceptions;
using TaskTracker.Application.Services;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Tests;

public class ProjectServiceTests
{
    private readonly IProjectRepository _repo = Substitute.For<IProjectRepository>();
    private readonly ProjectService _service;

    public ProjectServiceTests()
    {
        _service = new ProjectService(
            _repo, TimeProvider.System, NullLogger<ProjectService>.Instance);
    }

    [Fact]
    public async Task GetAsync_WhenProjectDoesNotExist_ThrowsNotFoundException()
    {
        _repo.GetByIdWithTasksAsync(5, Arg.Any<CancellationToken>())
             .Returns((Project?)null);

        await Assert.ThrowsAsync<NotFoundException>(
            () => _service.GetAsync(5, CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_TrimsNameAndSavesProject()
    {
        var request = new CreateProjectRequest("  Demo  ", "A test project");

        var result = await _service.CreateAsync(request, CancellationToken.None);

        Assert.Equal("Demo", result.Name);
        await _repo.Received(1).AddAsync(
            Arg.Is<Project>(p => p.Name == "Demo"),
            Arg.Any<CancellationToken>());
    }
}