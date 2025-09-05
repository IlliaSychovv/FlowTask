namespace FlowTask.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty; 
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }  
    public TaskStatus Status { get; set; }  
    public TaskPriority Priority { get; set; }  
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } 
    
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }
}

public enum TaskStatus
{
    Pending,
    InProgress,
    Completed
}

public enum TaskPriority
{
    Low, 
    Medium,
    High
}