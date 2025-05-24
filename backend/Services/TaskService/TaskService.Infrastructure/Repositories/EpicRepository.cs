using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;
using TaskService.Infrastructure.Database;
using TaskService.Infrastructure.Mappers;

namespace TaskService.Infrastructure.Repositories;

public class EpicRepository : IEpicRepository
{
    private readonly TaskDbContext _context;

    public EpicRepository(TaskDbContext context)
    {
        _context = context;
    }

    public async Task<Epic?> GetByIdAsync(Guid id)
    {
        return await _context.Epics
            .Include(e => e.Project)
            .Include(e => e.Tasks)
            .Select(e => e.Map())
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Epic>> GetByProjectIdAsync(Guid projectId)
    {
        return await _context.Epics
            .Include(e => e.Project)
            .Include(e => e.Tasks)
            .Where(e => e.ProjectId == projectId)
            .Select(e => e.Map())
            .ToListAsync();
    }

    public async Task AddAsync(Epic epic)
    {
        epic.CreatedAt = DateTime.UtcNow;
        epic.UpdatedAt = DateTime.UtcNow;
        
        await _context.Epics.AddAsync(epic.Map());
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Epic epic)
    {
        var existingEpic = await _context.Epics.FindAsync(epic.Id);
        if (existingEpic == null)
            throw new KeyNotFoundException($"Epic with ID {epic.Id} not found.");

        epic.CreatedAt = existingEpic.CreatedAt; // Preserve original creation date
        epic.UpdatedAt = DateTime.UtcNow;
        
        _context.Entry(existingEpic).CurrentValues.SetValues(epic);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var epic = await _context.Epics.FindAsync(id);
        if (epic != null)
        {
            _context.Epics.Remove(epic);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Epics.AnyAsync(e => e.Id == id);
    }
}