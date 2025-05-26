using UserService.Domain.Entities;

namespace UserService.Domain.Services;

public interface IUserService
{
    Task<User?> RegisterUserAsync(UserRegistration registration, CancellationToken ct);
}