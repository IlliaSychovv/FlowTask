using System.Security.Claims;
using FlowTask.Application.DTO.Task;
using FlowTask.Application.Interfaces.Repository;
using FlowTask.Application.Services;
using FlowTask.Domain.Entities;
using FlowTask.Domain.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shouldly;
using Moq;

namespace FlowTask.Tests.UnitTests;

public class TaskServiceTest
{
    private readonly Mock<ITaskRepository> _mockTaskRepository;
    private readonly Mock<ILogger<TaskService>> _mockLogger;
    private readonly Mock<IMemoryCache> _mockMemoryCache;
    private readonly TaskService _taskService;

    public TaskServiceTest()
    {
        _mockTaskRepository = new Mock<ITaskRepository>();
        _mockLogger = new Mock<ILogger<TaskService>>();
        _mockMemoryCache = new Mock<IMemoryCache>();

        _taskService = new TaskService(
            _mockTaskRepository.Object,
            _mockLogger.Object,
            _mockMemoryCache.Object
        );
    }

    private ClaimsPrincipal GetUser(Guid userId)
    {
        return new ClaimsPrincipal(
            new ClaimsIdentity(
                new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) }
            )
        );
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldCreateTask_AndInvalidateCache()
    {
        var userId = Guid.NewGuid();
        var user = GetUser(userId);
        var dto = new CreateTaskDto
        {
            Title = "Test",
            Description = "Desc",
            Status = 0,
            Priority = (FlowTaskPriority)1
        };

        _mockTaskRepository.Setup(x => x.CreateTaskAsync(It.IsAny<TaskItem>()))
            .Returns(Task.CompletedTask);

        var cacheRemoveCalled = false;
        _mockMemoryCache.Setup(x => x.Remove(It.IsAny<object>()))
            .Callback(() => cacheRemoveCalled = true);

        var result = await _taskService.CreateTaskAsync(dto, user);

        result.Title.ShouldBe(dto.Title);
        result.Description.ShouldBe(dto.Description);
        cacheRemoveCalled.ShouldBeTrue();
        _mockTaskRepository.Verify(x => x.CreateTaskAsync(It.IsAny<TaskItem>()), Times.Once);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturnTask_WhenUserHasAccess()
    {
        var userId = Guid.NewGuid();
        var user = GetUser(userId);
        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = "Test"
        };

        _mockTaskRepository.Setup(x => x.GetByIdAsync(task.Id))
            .ReturnsAsync(task);

        var result = await _taskService.GetTaskByIdAsync(task.Id, user);

        result.ShouldNotBeNull();
        result.Title.ShouldBe(task.Title);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldThrowTaskAccessException_WhenUserHasNoAccess()
    {
        var userId = Guid.NewGuid();
        var user = GetUser(userId);
        var task = new TaskItem { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Title = "Test" };

        _mockTaskRepository.Setup(x => x.GetByIdAsync(task.Id))
            .ReturnsAsync(task);

        await Should.ThrowAsync<TaskAccessException>(() =>
            _taskService.GetTaskByIdAsync(task.Id, user));
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldUpdateTask_AndInvalidateCache()
    {
        var userId = Guid.NewGuid();
        var user = GetUser(userId);
        var taskId = Guid.NewGuid();
        var task = new TaskItem { Id = taskId, UserId = userId, Title = "Old" };
        var dto = new UpdateTaskDto
        {
            Title = "New",
            Description = "Desc",
            Status = (FlowTaskStatus)1,
            Priority = (FlowTaskPriority)2
        };

        _mockTaskRepository.Setup(x => x.GetByIdAsync(taskId)).ReturnsAsync(task);
        _mockTaskRepository.Setup(x => x.UpdateTaskAsync(task)).Returns(Task.CompletedTask);

        var cacheRemoveCalled = false;
        _mockMemoryCache.Setup(x => x.Remove(It.IsAny<object>())).Callback(() => cacheRemoveCalled = true);

        var result = await _taskService.UpdateTaskAsync(taskId, dto, user);

        result.ShouldBeTrue();
        task.Title.ShouldBe(dto.Title);
        cacheRemoveCalled.ShouldBeTrue();
        _mockTaskRepository.Verify(x => x.UpdateTaskAsync(task), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldThrowTaskAccessException_WhenUserHasNoAccess()
    {
        var userId = Guid.NewGuid();
        var user = GetUser(userId);
        var taskId = Guid.NewGuid();
        var task = new TaskItem
        {
            Id = taskId,
            UserId = Guid.NewGuid(),
            Title = "Old"
        };
        var dto = new UpdateTaskDto
        {
            Title = "New"
        };

        _mockTaskRepository.Setup(x => x.GetByIdAsync(taskId)).ReturnsAsync(task);

        await Should.ThrowAsync<TaskAccessException>(() =>
            _taskService.UpdateTaskAsync(taskId, dto, user));
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldDeleteTaskAndInvalidateCache()
    {
        var userId = Guid.NewGuid();
        var user = GetUser(userId);
        var taskId = Guid.NewGuid();
        var task = new TaskItem { Id = taskId, UserId = userId };

        _mockTaskRepository.Setup(x => x.GetByIdAsync(taskId)).ReturnsAsync(task);
        _mockTaskRepository.Setup(x => x.DeleteTaskAsync(task)).Returns(Task.CompletedTask);

        var cacheRemoveCalled = false;
        _mockMemoryCache.Setup(x => x.Remove(It.IsAny<object>())).Callback(() => cacheRemoveCalled = true);

        var result = await _taskService.DeleteTaskAsync(taskId, user);

        result.ShouldBeTrue();
        cacheRemoveCalled.ShouldBeTrue();
        _mockTaskRepository.Verify(x => x.DeleteTaskAsync(task), Times.Once);
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldThrowTaskAccessException_WhenUserHasNoAccess()
    {
        var userId = Guid.NewGuid();
        var user = GetUser(userId);
        var taskId = Guid.NewGuid();
        var task = new TaskItem { Id = taskId, UserId = Guid.NewGuid() };

        _mockTaskRepository.Setup(x => x.GetByIdAsync(taskId)).ReturnsAsync(task);

        await Should.ThrowAsync<TaskAccessException>(() =>
            _taskService.DeleteTaskAsync(taskId, user));
    }
}