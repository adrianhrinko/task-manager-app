using TaskService.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Domain.Repositories;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid id);
    Task<IEnumerable<Project>> GetByOwnerId(Guid ownerId);
    Task<IEnumerable<Project>> GetByTeamId(Guid teamId);
    Task<Project> CreateAsync(Project project);
    Task<Project> UpdateAsync(Project project);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}