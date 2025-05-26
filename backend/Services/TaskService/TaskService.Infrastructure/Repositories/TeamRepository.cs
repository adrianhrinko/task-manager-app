using Shared.Domain.Entities;
using TaskService.Domain.Repositories;
using TaskService.Infrastructure.Database;
using TaskService.Infrastructure.Mappers;

namespace TaskService.Infrastructure.Repositories;

public class TeamRepository: ITeamRepository
{
    private readonly TaskDbContext _context;

    public TeamRepository(TaskDbContext context)
    {
        _context = context;
    }
    
    public async Task<Shared.Domain.Entities.Team> CreateAsync(Shared.Domain.Entities.Team team, CancellationToken ct)
    {
        
        var teamEntity = team.Map();
        _context.Teams.Add(teamEntity);
        
        
        await _context.SaveChangesAsync(ct);

        return teamEntity.Map();
    }

    public async Task<Shared.Domain.Entities.Team> UpdateAsync(Shared.Domain.Entities.Team team, CancellationToken ct)
    {
        var existingTeam = await _context.Teams.FindAsync(team.Id, ct);
        if (existingTeam == null)
            throw new KeyNotFoundException($"Team with ID {team.Id} not found");

        var newTeam = team.Map();

        _context.Entry(existingTeam).CurrentValues.SetValues(newTeam);
        await _context.SaveChangesAsync(ct);

        return newTeam.Map();
    }


    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var team = await _context.Teams.FindAsync(id, ct);
        if (team == null)
            throw new KeyNotFoundException($"Team with ID {id} not found");

        _context.Teams.Remove(team);
        await _context.SaveChangesAsync(ct);
    }
}