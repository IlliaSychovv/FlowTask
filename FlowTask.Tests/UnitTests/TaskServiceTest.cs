using FlowTask.Application.Interfaces.Repository;
using FlowTask.Application.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
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
    
}