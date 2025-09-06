using DomainTaskStatus = FlowTask.Domain.Entities.FlowTaskStatus;
using DomainTaskPriority = FlowTask.Domain.Entities.FlowTaskPriority;

namespace FlowTask.Application.DTO.Task;

public record TaskFilterDto
{
    public DomainTaskStatus? Status { get; set; }
    public DateTime? DueDate { get; set; }
    public DomainTaskPriority? Priority { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = false;
}