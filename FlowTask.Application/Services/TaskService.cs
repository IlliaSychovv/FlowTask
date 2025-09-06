using System.Security.Claims;
using FlowTask.Application.DTO;
using FlowTask.Application.DTO.Task;
using FlowTask.Application.Interfaces.Repository;
using FlowTask.Application.Interfaces.Service;
using FlowTask.Domain.Entities;
using SequentialGuid;
using Mapster;

namespace FlowTask.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskDto> CreateTaskAsync(CreateTaskDto dto, ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        var task = dto.Adapt<TaskItem>();
        task.Priority = dto.Priority;
        task.Status = dto.Status;
        task.Id = SequentialGuidGenerator.Instance.NewGuid(); 
        task.CreatedAt = DateTime.UtcNow; 
        task.UserId = Guid.Parse(userIdClaim);
        
        if (task.DueDate.HasValue)
            task.DueDate = DateTime.SpecifyKind(task.DueDate.Value, DateTimeKind.Utc);

        await _taskRepository.CreateTaskAsync(task);
        return task.Adapt<TaskDto>();
    }

    public async Task<TaskDto> GetTaskByIdAsync(Guid id, ClaimsPrincipal user)
    {
        var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var task = await _taskRepository.GetByIdAsync(id);
        
        if (task == null || task.UserId != userId)
            return null;
        
        return task.Adapt<TaskDto>();
    }

    public async Task<bool> UpdateTaskAsync(Guid id, UpdateTaskDto dto, ClaimsPrincipal user)
    {
        var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null || task.UserId != userId)
            return false; 
        
        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Status = dto.Status;
        task.Priority = dto.Priority;
        task.UpdatedAt = DateTime.UtcNow;
        
        await _taskRepository.UpdateTaskAsync(task);
        return true;
    }

    public async Task<bool> DeleteTaskAsync(Guid id, ClaimsPrincipal user)
    {
        var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null || task.UserId != userId)
            return false;
        
        await _taskRepository.DeleteTaskAsync(task);
        return true;
    }

    public async Task<PagedResponse<TaskDto>> GetTasksAsync(ClaimsPrincipal user, TaskFilterDto? filter = null,
        int pageNumber = 1, int pageSize = 10)
    {
        var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        int skip = (pageNumber - 1) * pageSize;
        int take = pageSize;
        
        var tasks = await _taskRepository.GetTasksAsync(userId, skip, take, filter);
        var totalCount = await _taskRepository.CountAsync(userId, filter);
        
        var item = tasks.Adapt<List<TaskDto>>();

        return new PagedResponse<TaskDto>()
        {
            Items = item,
            CurrentPage = pageNumber,
            TotalCount = totalCount,
            PageSize = pageSize
        };
    }
}