namespace FlowTask.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty; 
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }  
    public FlowTaskStatus Status { get; set; }  
    public FlowTaskPriority Priority { get; set; }  
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } 
    
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }
}

public enum FlowTaskStatus
{
    Pending,
    InProgress,
    Completed
}

public enum FlowTaskPriority
{
    Low, 
    Medium,
    High
}