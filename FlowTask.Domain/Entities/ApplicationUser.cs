using Microsoft.AspNetCore.Identity;

namespace FlowTask.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }  
    
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}