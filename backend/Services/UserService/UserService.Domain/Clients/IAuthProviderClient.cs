using System.Net;
using UserService.Domain.Entities;

namespace UserService.Domain.Clients;

public interface IAuthProviderClient
{
    Task<RegistrationResult> RegisterUserAsync(UserRegistration registration, CancellationToken ct);
    
    Task<AuthToken?> LoginUserAsync(UserLogin login, CancellationToken ct);
    
    Task<AuthToken?> RefreshTokenAsync(string refreshToken, CancellationToken ct);
}

public enum RegistrationResult
{
    Success,
    Conflict,
    Failure
}