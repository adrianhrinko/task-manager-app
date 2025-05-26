using Shared.Domain.Entities;
using TaskService.Domain.Repositories;
using TaskService.Infrastructure.Database;
using TaskService.Infrastructure.Mappers;

namespace TaskService.Infrastructure.Repositories;

public class UserRepository: IUserRepository
{
    private readonly TaskDbContext _context;

    public UserRepository(TaskDbContext context)
    {
        _context = context;
    }
    
    public async Task<Shared.Domain.Entities.User> CreateAsync(Shared.Domain.Entities.User user, CancellationToken ct)
    {
        
        var userEntity = user.Map();
        _context.Users.Add(userEntity);
        
        
        await _context.SaveChangesAsync(ct);

        return userEntity.Map();
    }

    public async Task<Shared.Domain.Entities.User> UpdateAsync(Shared.Domain.Entities.User user, CancellationToken ct)
    {
        var existingUser = await _context.Users.FindAsync(user.Id, ct);
        if (existingUser == null)
            throw new KeyNotFoundException($"User with ID {user.Id} not found");

        var newUser = user.Map();

        _context.Entry(existingUser).CurrentValues.SetValues(newUser);
        await _context.SaveChangesAsync(ct);

        return newUser.Map();
    }


    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var user = await _context.Users.FindAsync(id, ct);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(ct);
    }
}