using FlowTask.Application.DTO.Task;
using FlowTask.Domain.Entities;

namespace FlowTask.Application.Interfaces.Repository;

public interface ITaskRepository
{
    Task CreateTaskAsync(TaskItem task);
    Task<IReadOnlyList<TaskItem>> GetTasksAsync(Guid userId); 
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task UpdateTaskAsync(TaskItem task);
    Task DeleteTaskAsync(TaskItem task);
}