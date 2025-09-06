using System.Security.Claims;
using FlowTask.Application.DTO;
using FlowTask.Application.DTO.Task;

namespace FlowTask.Application.Interfaces.Service;

public interface ITaskService
{
    Task<TaskDto> CreateTaskAsync(CreateTaskDto dto, ClaimsPrincipal user);
    Task<TaskDto> GetTaskByIdAsync(Guid id, ClaimsPrincipal user);
    Task<bool> UpdateTaskAsync(Guid id, UpdateTaskDto dto, ClaimsPrincipal user);
    Task<bool> DeleteTaskAsync(Guid id, ClaimsPrincipal user);
    Task<PagedResponse<TaskDto>> GetTasksAsync(ClaimsPrincipal user, TaskFilterDto? filter = null,
        int pageNumber = 1, int pageSize = 10);
}