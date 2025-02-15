using Shared;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;

namespace UserService.API.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/users");

        group.MapGet("/{id:guid}", async (Guid id, IUserRepository userRepository) =>
        {
            var user = await userRepository.GetByIdAsync(id);
            return user is not null ? Results.Ok(user) : Results.NotFound();
        })
        .WithName("GetUserById");

        group.MapGet("/by-email/{email}", async (string email, IUserRepository userRepository) =>
        {
            var user = await userRepository.GetByEmailAsync(email);
            return user is not null ? Results.Ok(user) : Results.NotFound();
        })
        .WithName("GetUserByEmail");

        group.MapGet("/", async ([AsParameters] Query query, IUserRepository userRepository) =>
        {
            var users = await userRepository.GetAllAsync(query);
            return Results.Ok(users);
        })
        .WithName("GetAllUsers");

        group.MapPost("/", async (User user, IUserRepository userRepository) =>
        {
            var createdUser = await userRepository.CreateAsync(user);
            return Results.Created($"/api/users/{createdUser.Id}", createdUser);
        })
        .WithName("CreateUser");

        group.MapPut("/{id:guid}", async (Guid id, User user, IUserRepository userRepository) =>
        {
            if (id != user.Id)
            {
                return Results.BadRequest("User ID mismatch.");
            }

            var exists = await userRepository.ExistsAsync(id);
            if (!exists)
            {
                return Results.NotFound();
            }

            var updatedUser = await userRepository.UpdateAsync(user);
            return Results.Ok(updatedUser);
        })
        .WithName("UpdateUser");

        group.MapDelete("/{id:guid}", async (Guid id, IUserRepository userRepository) =>
        {
            var exists = await userRepository.ExistsAsync(id);
            if (!exists)
            {
                return Results.NotFound();
            }

            await userRepository.DeleteAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteUser");
        
        return routes;
    }
}