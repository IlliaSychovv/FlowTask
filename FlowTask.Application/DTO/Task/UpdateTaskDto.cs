using FlowTask.Domain.Entities;

namespace FlowTask.Application.DTO.Task;

public record UpdateTaskDto
{ 
    public string Title { get; set; } = string.Empty; 
    public string? Description { get; set; }
    public FlowTaskPriority Priority { get; set; } = FlowTaskPriority.Low;
    public FlowTaskStatus Status { get; set; } = FlowTaskStatus.Pending;
}