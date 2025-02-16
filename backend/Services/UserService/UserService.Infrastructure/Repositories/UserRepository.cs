using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.Domain;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Database;
using UserService.Infrastructure.Database.Entities;
using UserService.Infrastructure.Mappers;

namespace UserService.Infrastructure.Repositories;

public class UserRepository(UserDbContext context) : IUserRepository
{
    public async Task<Domain.Entities.User?> GetByIdAsync(Guid id)
    {
        return await context.Users
            .AsNoTracking()
            .Include(u => u.UserTeamRoles)
            .ThenInclude(utr => utr.Team)
            .Select(u => u.Map())
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Domain.Entities.User?> GetByEmailAsync(string email)
    {
        return await context.Users
            .AsNoTracking()
            .Include(u => u.UserTeamRoles)
            .ThenInclude(utr => utr.Team)
            .Select(u => u.Map())
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<PagedList<Domain.Entities.User>> GetAllAsync(Query query)
    {
        IQueryable<User> userQuery = context.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Filter))
        {
            userQuery = userQuery.Where(u =>
                u.FirstName.Contains(query.Filter) ||
                u.LastName.Contains(query.Filter) || 
                u.Email.Contains(query.Filter));
        }

        userQuery = query.Order is Order.Desc ? 
            userQuery.OrderByDescending(GetSortProperty(query)) : 
            userQuery.OrderBy(GetSortProperty(query));

        var total = await userQuery.CountAsync();
        var result = await userQuery
            .Skip((query.PageNo - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(u => u.Map())
            .ToListAsync();

        return new PagedList<Domain.Entities.User>(result, query.PageNo, query.PageSize, total);
    }

    public async Task<Domain.Entities.User> CreateAsync(Domain.Entities.User user)
    {
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        
        context.Users.Add(user.Map());
        await context.SaveChangesAsync();
        
        return user;
    }

    public async Task<Domain.Entities.User> UpdateAsync(Domain.Entities.User user)
    {
        var existingUser = await context.Users.FindAsync(user.Id);
        if (existingUser == null)
            throw new KeyNotFoundException($"User with ID {user.Id} not found");

        var newUser = user.Map();

        newUser.CreatedAt = existingUser.CreatedAt;
        newUser.UpdatedAt = DateTime.UtcNow;

        context.Entry(existingUser).CurrentValues.SetValues(newUser);
        await context.SaveChangesAsync();

        return newUser.Map();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found");

        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.Users.AsNoTracking().AnyAsync(u => u.Id == id);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await context.Users.AsNoTracking().AnyAsync(u => u.Email == email);
    }
    
    private static Expression<Func<User, object>> GetSortProperty(Query query) =>
        query.OrderBy?.ToLower() switch
        {
            "name" => user => user.FirstName + " " + user.LastName,
            "email" => product => product.Email,
            _ => product => product.Id
        };
}