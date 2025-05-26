using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.Domain;
using Shared.Infrastructure;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Database;
using UserService.Infrastructure.Database.Entities;
using UserService.Infrastructure.Mappers;

namespace UserService.Infrastructure.Repositories;

public class UserRepository(UserDbContext context) : IUserRepository
{
    public async Task<Domain.Entities.User?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var userEntity = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, ct);

        return userEntity?.Map();
    }

    public async Task<Domain.Entities.User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        var userEntity = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, ct);
        
        return userEntity?.Map();
    }

    public Task<PagedList<Domain.Entities.User>> GetAllAsync(Query query, CancellationToken ct)
    {
        IQueryable<Database.Entities.User> userQuery = context.Users.AsNoTracking();
        
        var sortMappings = new Dictionary<string, Expression<Func<Database.Entities.User, object>>>
        {
            { "name", u =>  new { u.FirstName, u.LastName }},
            { "email", u => u.Email },
            { "createdAt", u => u.CreatedAt },
        };
        
        var filterMappings = new Dictionary<string, Expression<Func<Database.Entities.User, bool>>>
        {
            { "name", u =>  u.FirstName.Contains(query.Filter) || u.LastName.Contains(query.Filter) },
            { "email", u => u.Email.Contains(query.Filter) }
        };

        userQuery = userQuery.ApplyQuery(query, filterMappings, sortMappings);
        
        return userQuery.ApplyPaging<Database.Entities.User, Domain.Entities.User>(query, u => u.Map(), ct);
    }
    
    public Task<PagedList<Domain.Entities.Team>> GetTeamsAsync(Guid userId, Query query, CancellationToken ct)
    {
        var teamsQuery = context.UserTeamRoles
            .AsNoTracking()
            .Include(utr => utr.Team)
            .Where(utr => utr.UserId == userId);

        var sortMappings = new Dictionary<string, Expression<Func<UserTeamRole, object>>>
        {
            { "name", utr => utr.Team.Name },
            { "role", utr => utr.Role },
            { "assigned", utr => utr.AssignedDate }
        };
        
        var filterMappings = new Dictionary<string, Expression<Func<UserTeamRole, bool>>>
        {
            { "name", utr => utr.Team.Name.Contains(query.Filter) },
            { "role", utr => utr.Role.ToString().Contains(query.Filter) },
        };
        
        teamsQuery = teamsQuery.ApplyQuery(query, filterMappings, sortMappings);
        
        return teamsQuery.ApplyPaging<UserTeamRole, Domain.Entities.Team>(query, utr => utr.MapToTeam(), ct);
    }

    public async Task<Domain.Entities.User> CreateAsync(Domain.Entities.User user, CancellationToken ct)
    {
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        
        var userEntity = user.Map();
        var entry = context.Users.Add(userEntity);
        await context.SaveChangesAsync(ct);
        
        return entry.Entity.Map();
    }

    public async Task<Domain.Entities.User> UpdateAsync(Domain.Entities.User user, CancellationToken ct)
    {
        var existingUser = await context.Users.FindAsync(user.Id, ct);
        if (existingUser == null)
            throw new KeyNotFoundException($"User with ID {user.Id} not found");

        var newUser = user.Map();

        newUser.CreatedAt = existingUser.CreatedAt;
        newUser.UpdatedAt = DateTime.UtcNow;

        context.Entry(existingUser).CurrentValues.SetValues(newUser);
        await context.SaveChangesAsync(ct);

        return newUser.Map();
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found");

        context.Users.Remove(user);
        await context.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct)
    {
        return await context.Users.AsNoTracking().AnyAsync(u => u.Id == id, ct);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
    {
        return await context.Users.AsNoTracking().AnyAsync(u => u.Email == email, ct);
    }
}