using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.Domain;
using Shared.Infrastructure;
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
        var teamEntity =  await context.Teams
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
        
        return teamEntity?.Map();
    }

    public  Task<PagedList<Domain.Entities.Team>> GetAllAsync(Query query)
    {
        var teamQuery = context.Teams.AsNoTracking();
        
        var sortMappings = new Dictionary<string, Expression<Func<Team, object>>>
        {
            { "name", t => t.Name },
            { "createdAt", t => t.CreatedAt },
        };
        
        var filterMappings = new Dictionary<string, Expression<Func<Team, bool>>>
        {
            { "name", t => t.Name.Contains(query.Filter) },
        };
        
        teamQuery = teamQuery.ApplyQuery<Team>(query, filterMappings, sortMappings);

        return teamQuery.ApplyPaging(query, t => t.Map());
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
    
    public Task<PagedList<Domain.Entities.TeamMember>> GetTeamMembersAsync(Guid teamId, Query query)
    {
        var memberQuery = context.UserTeamRoles
            .AsNoTracking()
            .Include(utr => utr.User)
            .Where(utr => utr.TeamId == teamId);

        var sortMappings = new Dictionary<string, Expression<Func<UserTeamRole, object>>>
        {
            { "name", utr => new { utr.User.FirstName, utr.User.LastName } },
            { "email", utr => utr.User.Email },
            { "role", utr => utr.Role },
            { "assigned", utr => utr.AssignedDate }
        };
        
        var filterMappings = new Dictionary<string, Expression<Func<UserTeamRole, bool>>>
        {
            { "name", utr => utr.User.FirstName.Contains(query.Filter) || utr.User.LastName.Contains(query.Filter)},
            { "email", utr => utr.User.Email.Contains(query.Filter) },
            { "role", utr => utr.Role.ToString().Contains(query.Filter) },
        };
        
        memberQuery = memberQuery.ApplyQuery(query, filterMappings, sortMappings);
        
        return memberQuery.ApplyPaging(query, m => m.MapToTeamMember());
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
}