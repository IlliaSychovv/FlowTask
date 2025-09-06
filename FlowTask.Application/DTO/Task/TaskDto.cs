using FlowTask.Domain.Entities;

namespace FlowTask.Application.DTO.Task;

public record TaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty; 
    public string? Description { get; set; }
    public FlowTaskStatus Status { get; set; }  
    public FlowTaskPriority Priority { get; set; }
    public DateTime? DueDate { get; set; }  
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } 
}