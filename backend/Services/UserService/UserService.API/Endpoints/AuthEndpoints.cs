using UserService.API.DTOs.Auth;
using UserService.API.Mappers;
using UserService.Domain.Services;

namespace UserService.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("auth");
        
        group.MapPost("/register", async (UserRegistrationDto registration, IAuthService authService) =>
        {
            var result = await authService.RegisterUserAsync(registration.Map());
            return result is not null ? Results.Ok(result.Map()) : Results.BadRequest("Failed to register user");
        });

        group.MapPost("/login", async (UserLoginDto login, IAuthService authService) =>
        {
            var authToken = await authService.LoginUserAsync(login.Map());
            return authToken;
        });
        
        group.MapPost("/refresh", async (RefreshTokenDto refreshToken, IAuthService authService) =>
        {
            var token = await authService.RefreshTokenAsync(refreshToken.Token);
            return token != null ? Results.Ok(new { access_token = token }) : Results.Unauthorized();
        });
    }
}