namespace FlowTask.Application.DTO;

public record PagedResponse<T>
{
    public List<T> Items { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}