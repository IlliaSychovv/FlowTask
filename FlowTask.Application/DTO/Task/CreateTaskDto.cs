using FlowTask.Domain.Entities;

namespace FlowTask.Application.DTO.Task;

public record CreateTaskDto
{
    public string Title { get; set; } = string.Empty; 
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public FlowTaskPriority Priority { get; set; } = FlowTaskPriority.Low;
    public FlowTaskStatus Status { get; set; } = FlowTaskStatus.Pending;
}