using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;
using TaskService.Infrastructure.Database;
using TaskService.Infrastructure.Mappers;

namespace TaskService.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly TaskDbContext _context;

    public ProjectRepository(TaskDbContext context)
    {
        _context = context;
    }

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await _context.Projects
            .Include(p => p.TaskItems)
            .Include(p => p.Epics)
            .Select(p => p.Map())
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Project>> GetByOwnerId(Guid ownerId)
    {
        return await _context.Projects
            .Include(p => p.TaskItems)
            .Include(p => p.Epics)
            .Where(p => p.OwnerId == ownerId)
            .Select(p => p.Map())
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetByTeamId(Guid teamId)
    {
        return await _context.Projects
            .Include(p => p.TaskItems)
            .Include(p => p.Epics)
            .Where(p => p.TeamId == teamId)
            .Select(p => p.Map())
            .ToListAsync();
    }

    public async Task<Project> CreateAsync(Project project)
    {
        project.CreatedAt = DateTime.UtcNow;
        project.UpdatedAt = DateTime.UtcNow;
        
        var entry = await _context.Projects.AddAsync(project.Map());
        await _context.SaveChangesAsync();
        
        return entry.Entity.Map();
    }

    public async Task<Project> UpdateAsync(Project project)
    {
        var existingProject = await _context.Projects.FindAsync(project.Id);
        if (existingProject == null)
            throw new KeyNotFoundException($"Project with ID {project.Id} not found.");

        project.CreatedAt = existingProject.CreatedAt; // Preserve original creation date
        project.UpdatedAt = DateTime.UtcNow;
        
        _context.Entry(existingProject).CurrentValues.SetValues(project);
        await _context.SaveChangesAsync();
        
        return existingProject.Map();
    }

    public async Task DeleteAsync(Guid id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project != null)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Projects.AnyAsync(p => p.Id == id);
    }
}