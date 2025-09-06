using FlowTask.Application.DTO.Task;
using FlowTask.Application.Interfaces.Repository;
using FlowTask.Domain.Entities;
using FlowTask.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlowTask.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task CreateTaskAsync(TaskItem task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<TaskItem>> GetTasksAsync(Guid userId, int skip, int take, TaskFilterDto? filter = null)
    {
        var query = _context.Tasks
            .Where(t => t.UserId == userId)
            .AsQueryable()
            .AsNoTracking();

        if (filter != null)
        {
            if (filter.Status.HasValue)
                query = query.Where(t => t.Status == filter.Status.Value);

            if (filter.DueDate.HasValue)
                query = query.Where(t => t.DueDate.HasValue && 
                                         t.DueDate.Value.Date <= filter.DueDate.Value.Date);

            if (filter.Priority.HasValue)
                query = query.Where(t => t.Priority == filter.Priority.Value);

            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                query = filter.SortBy.ToLower() switch
                {
                    "duedate" => filter.SortDescending
                        ? query.OrderByDescending(t => t.DueDate)
                        : query.OrderBy(t => t.DueDate),
                    "priority" => filter.SortDescending
                        ? query.OrderByDescending(t => t.Priority)
                        : query.OrderBy(t => t.Priority),
                    _ => query
                };
            }
        }
        
        query = query.
            Skip(skip).
            Take(take);

        return await query.ToListAsync();
    }

    public async Task<int> CountAsync(Guid userId, TaskFilterDto? filter = null)
    {
        var query = _context.Tasks
            .Where(t => t.UserId == userId)
            .AsQueryable();

        if (filter != null)
        {
            if (filter.Status.HasValue)
                query = query.Where(t => t.Status == filter.Status.Value);
            
            if (filter.DueDate.HasValue)
                query = query.Where(t => t.DueDate.HasValue && 
                                         t.DueDate.Value.Date <= filter.DueDate.Value.Date);
            
            if (filter.Priority.HasValue)
                query = query.Where(t => t.Priority == filter.Priority.Value);
        }
        
        return await query.CountAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return await _context.Tasks
            .FindAsync(id);
    }

    public async Task UpdateTaskAsync(TaskItem task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(TaskItem task)
    {
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }
}