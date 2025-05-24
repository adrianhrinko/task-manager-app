using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;
using TaskService.Infrastructure.Mappers;

namespace TaskService.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TaskDbContext _context;

    public TaskRepository(TaskDbContext context)
    {
        _context = context;
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return await _context.Tasks
            .Select(t => t.Map())
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<TaskItem>> GetByEpicIdAsync(Guid epicId)
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .Where(t => t.EpicId == epicId)
            .Select(t => t.Map())
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetByProjectIdAsync(Guid projectId)
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .Where(t => t.ProjectId == projectId)
            .Select(t => t.Map())
            .ToListAsync();
    }

    public async Task<TaskItem> AddAsync(TaskItem task)
    {
        task.CreatedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;
        
        var entry = await _context.Tasks.AddAsync(task.Map());
        await _context.SaveChangesAsync();

        return entry.Entity.Map();
    }

    public async Task<Domain.Entities.TaskItem> UpdateAsync(Domain.Entities.TaskItem task)
    {
        var existingTask = await _context.Tasks.FindAsync(task.Id);
        if (existingTask == null)
            throw new KeyNotFoundException($"Task with ID {task.Id} not found.");

        task.CreatedAt = existingTask.CreatedAt; // Preserve original creation date
        task.UpdatedAt = DateTime.UtcNow;
        
        _context.Entry(existingTask).CurrentValues.SetValues(task);
        await _context.SaveChangesAsync();
        
        return existingTask.Map();
    }

    public async Task DeleteAsync(Guid id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Tasks.AnyAsync(t => t.Id == id);
    }
}