using TaskService.Domain.Entities;

namespace TaskService.Domain.Repositories;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<IEnumerable<TaskItem>> GetByEpicIdAsync(Guid epicId);
    Task<IEnumerable<TaskItem>> GetByProjectIdAsync(Guid projectId);
    Task<TaskItem> AddAsync(TaskItem task);
    Task<TaskItem> UpdateAsync(TaskItem task);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}