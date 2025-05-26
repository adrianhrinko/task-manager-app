using TaskService.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Domain.Repositories;

public interface IUserRepository
{
    Task<Shared.Domain.Entities.User> CreateAsync(Shared.Domain.Entities.User user, CancellationToken ct);
    
    Task<Shared.Domain.Entities.User> UpdateAsync(Shared.Domain.Entities.User user, CancellationToken ct);
        
    Task DeleteAsync(Guid id, CancellationToken ct);
}
