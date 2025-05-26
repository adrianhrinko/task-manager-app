using UserService.API.DTOs.Auth;
using UserService.API.Mappers;
using UserService.Domain.Clients;
using UserService.Domain.Services;

namespace UserService.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("auth");
        
        group.MapPost("/register", async (UserRegistrationDto registration, IUserService userService, CancellationToken ct) =>
        {
            var result = await userService.RegisterUserAsync(registration.Map(), ct);
            return result is not null ? Results.Ok(result.Map()) : Results.BadRequest("Failed to register user");
        });

        group.MapPost("/login", async (UserLoginDto login, IAuthProviderClient authService, CancellationToken ct) =>
        {
            var authToken = await authService.LoginUserAsync(login.Map(), ct);
            return authToken;
        });
        
        group.MapPost("/refresh", async (RefreshTokenDto refreshToken, IAuthProviderClient authService, CancellationToken ct) =>
        {
            var token = await authService.RefreshTokenAsync(refreshToken.Token, ct);
            return token != null ? Results.Ok(new { access_token = token }) : Results.Unauthorized();
        });
    }
}