using UserService.Domain.Entities;

namespace UserService.Domain.Services;

public interface IAuthService
{
    Task<bool> RegisterUserAsync(UserRegistration registration);
    Task<AuthToken> LoginUserAsync(UserLogin login);
    
    Task<string?> RefreshTokenAsync(string refreshToken);
}