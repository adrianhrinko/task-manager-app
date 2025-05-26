using TaskService.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Domain.Repositories;

public interface ITeamRepository
{
    Task<Shared.Domain.Entities.Team> CreateAsync(Shared.Domain.Entities.Team team, CancellationToken ct);
    
    Task<Shared.Domain.Entities.Team> UpdateAsync(Shared.Domain.Entities.Team team, CancellationToken ct);
        
    Task DeleteAsync(Guid id, CancellationToken ct);
}
