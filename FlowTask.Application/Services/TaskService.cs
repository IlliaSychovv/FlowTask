using System.Security.Claims;
using FlowTask.Application.DTO;
using FlowTask.Application.DTO.Task;
using FlowTask.Application.Interfaces.Repository;
using FlowTask.Application.Interfaces.Service;
using Microsoft.Extensions.Logging;
using FlowTask.Domain.Entities;
using FlowTask.Domain.Exceptions;
using SequentialGuid;
using Mapster;
using Microsoft.Extensions.Caching.Memory;

namespace FlowTask.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ILogger<TaskService> _logger;
    private readonly IMemoryCache _cache;

    public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger, IMemoryCache cache)
    {
        _taskRepository = taskRepository;
        _logger = logger;
        _cache = cache;
    }

    public async Task<TaskDto> CreateTaskAsync(CreateTaskDto dto, ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = Guid.Parse(userIdClaim);
        
        var task = dto.Adapt<TaskItem>();
        task.Id = SequentialGuidGenerator.Instance.NewGuid(); 
        task.UserId = userId;
        
        if (task.DueDate.HasValue)
            task.DueDate = DateTime.SpecifyKind(task.DueDate.Value, DateTimeKind.Utc);
        
        _logger.LogInformation("Task {TaskId} created by User {UserId}", task.Id, userId);

        await _taskRepository.CreateTaskAsync(task);
        InvalidateUserTasksCache(userId);
        return task.Adapt<TaskDto>();
    }

    public async Task<TaskDto> GetTaskByIdAsync(Guid id, ClaimsPrincipal user)
    {
        var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var task = await _taskRepository.GetByIdAsync(id);

        if (task == null || task.UserId != userId)
        {
            _logger.LogWarning("User {UserId} attempted to access Task {TaskId} without permission", userId, id);
            throw new TaskAccessException();
        }
        
        return task.Adapt<TaskDto>();
    }

    public async Task<bool> UpdateTaskAsync(Guid id, UpdateTaskDto dto, ClaimsPrincipal user)
    {
        var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null || task.UserId != userId)
        {
            _logger.LogWarning("User {UserId} attempted to update Task {TaskId} without permission", userId, id);
            throw new TaskAccessException();
        }
        
        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Status = dto.Status;
        task.Priority = dto.Priority;
        task.UpdatedAt = DateTime.UtcNow;
        
        await _taskRepository.UpdateTaskAsync(task);
        InvalidateUserTasksCache(userId);
        return true;
    }

    public async Task<bool> DeleteTaskAsync(Guid id, ClaimsPrincipal user)
    {
        var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null || task.UserId != userId)
            throw new TaskAccessException();
        
        _logger.LogInformation("Task {TaskId} deleted by User {UserId}", id, userId);
        
        await _taskRepository.DeleteTaskAsync(task);
        InvalidateUserTasksCache(userId);
        return true;
    }

    public async Task<PagedResponse<TaskDto>> GetTasksAsync(ClaimsPrincipal user, TaskFilterDto? filter = null,
        int pageNumber = 1, int pageSize = 10)
    {
        var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        string cashKey = $"user_{userId}_tasks";
       
        if (!_cache.TryGetValue(cashKey, out List<TaskDto>? cachedTasks))
        {
            var tasks = await _taskRepository.GetTasksAsync(userId);
            cachedTasks = tasks.Adapt<List<TaskDto>>();

            _cache.Set(cashKey, cachedTasks, TimeSpan.FromMinutes(60));
        }
        
        IEnumerable<TaskDto> filtered = cachedTasks;

        if (filter != null)
        {
            if (filter.Status.HasValue)
                filtered = filtered.Where(t => t.Status == filter.Status.Value);
            if (filter.Priority.HasValue)
                filtered = filtered.Where(t => t.Priority == filter.Priority.Value);
            if (filter.DueDate.HasValue)
                filtered = filtered.Where(t => t.DueDate.HasValue &&
                                               t.DueDate.Value.Date <= filter.DueDate.Value.Date);

            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                filtered = filter.SortBy.ToLower() switch
                {
                    "duedate" => filter.SortDescending ? filtered.OrderByDescending(t => t.DueDate)
                        : filtered.OrderBy(t => t.DueDate),
                    "priority" => filter.SortDescending ? filtered.OrderByDescending(t => t.Priority)
                        : filtered.OrderBy(t => t.Priority),
                    _ => filtered
                };
            }
        }
        
        var paged = filtered
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        
        _logger.LogInformation("User {UserId} fetched {Count} tasks (page {Page})", userId, paged.Count, pageNumber);

        return new PagedResponse<TaskDto>
        {
            Items = paged,
            CurrentPage = pageNumber,
            TotalCount = filtered.Count(),
            PageSize = pageSize
        };
    }
    
    private void InvalidateUserTasksCache(Guid userId)
    {
        string cacheKey = $"user_{userId}_tasks";
        _cache.Remove(cacheKey);
    }
}