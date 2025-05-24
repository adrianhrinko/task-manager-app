using TaskService.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Domain.Repositories;

public interface IEpicRepository
{
    Task<Epic?> GetByIdAsync(Guid id);
    Task<IEnumerable<Epic>> GetByProjectIdAsync(Guid projectId);
    Task AddAsync(Epic epic);
    Task UpdateAsync(Epic epic);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}