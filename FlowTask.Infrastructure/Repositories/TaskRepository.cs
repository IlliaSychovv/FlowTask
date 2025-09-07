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

    public async Task<IReadOnlyList<TaskItem>> GetTasksAsync(Guid userId)
    {
        return await _context.Tasks
            .Where(t => t.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
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