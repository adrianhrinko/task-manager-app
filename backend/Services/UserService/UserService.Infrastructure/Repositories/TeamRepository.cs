using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.Domain;
using UserService.Domain.Enums;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Database;
using UserService.Infrastructure.Database.Entities;
using UserService.Infrastructure.Mappers;

namespace UserService.Infrastructure.Repositories;

public class TeamRepository(UserDbContext context) : ITeamRepository
{
    public async Task<Domain.Entities.Team?> GetByIdAsync(Guid id)
    {
        return await context.Teams
            .AsNoTracking()
            .Select(t => t.Map())
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<PagedList<Domain.Entities.Team>> GetAllAsync(Query query)
    {
        IQueryable<Team> teamQuery = context.Teams.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Filter))
        {
            teamQuery = teamQuery.Where(t =>
                t.Name.Contains(query.Filter) ||
                t.Description.Contains(query.Filter));
        }

        teamQuery = query.Order is Order.Desc ? 
            teamQuery.OrderByDescending(GetSortProperty(query)) : 
            teamQuery.OrderBy(GetSortProperty(query));

        var total = await teamQuery.CountAsync();
        var result = await teamQuery
            .Skip((query.PageNo - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(t => t.Map())
            .ToListAsync();

        return new PagedList<Domain.Entities.Team>(result, query.PageNo, query.PageSize, total);
    }

    public async Task<Domain.Entities.Team> CreateAsync(Guid userId, Domain.Entities.Team team)
    {
        team.CreatedAt = DateTime.UtcNow;
        team.UpdatedAt = DateTime.UtcNow;
        
        var teamEntity = team.Map();
        context.Teams.Add(teamEntity);
        
        context.UserTeamRoles.Add(new UserTeamRole
        {
            UserId = userId,
            Team = teamEntity,
            Role = TeamRole.Owner
        });
        
        await context.SaveChangesAsync();

        return teamEntity.Map();
    }

    public async Task<Domain.Entities.Team> UpdateAsync(Domain.Entities.Team team)
    {
        var existingTeam = await context.Teams.FindAsync(team.Id);
        if (existingTeam == null)
            throw new KeyNotFoundException($"Team with ID {team.Id} not found");

        var newTeam = team.Map();

        newTeam.CreatedAt = existingTeam.CreatedAt;
        newTeam.UpdatedAt = DateTime.UtcNow;

        context.Entry(existingTeam).CurrentValues.SetValues(newTeam);
        await context.SaveChangesAsync();

        return newTeam.Map();
    }

    public async Task DeleteAsync(Guid id)
    {
        var team = await context.Teams.FindAsync(id);
        if (team == null)
            throw new KeyNotFoundException($"Team with ID {id} not found");

        context.Teams.Remove(team);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.Teams.AsNoTracking().AnyAsync(t => t.Id == id);
    }

    private static Expression<Func<Team, object>> GetSortProperty(Query query) =>
        query.OrderBy?.ToLower() switch
        {
            "name" => team => team.Name,
            "createdat" => team => team.CreatedAt,
            _ => team => team.Id
        };
} 