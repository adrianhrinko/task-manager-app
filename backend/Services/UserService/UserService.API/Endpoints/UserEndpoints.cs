using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Domain;
using UserService.API.DTOs.Users;
using UserService.API.Mappers;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;

namespace UserService.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("users");
        
        group.MapGet("/{id:guid}", async (Guid id, [FromServices] IUserRepository userRepository, CancellationToken ct) =>
        {
            var user = await userRepository.GetByIdAsync(id, ct);
            return user is not null ? Results.Ok(user.Map()) : Results.NotFound();
        })
        .WithName("GetUserById");

        group.MapGet("/by-email/{email}", async (string email, [FromServices] IUserRepository userRepository, CancellationToken ct) =>
        {
            var user = await userRepository.GetByEmailAsync(email, ct);
            return user is not null ? Results.Ok(user.Map()) : Results.NotFound();
        })
        .WithName("GetUserByEmail");

        group.MapGet("/", async ([AsParameters] Query query, [FromServices] IUserRepository userRepository, CancellationToken ct) =>
            {
                var users = await userRepository.GetAllAsync(query, ct);
                return Results.Ok(users.MapItems(u => u.Map()));
            })
            .WithName("GetAllUsers");
        
        group.MapGet("/{id:guid}/teams", async (Guid id, [AsParameters] Query query, [FromServices] IUserRepository userRepository, CancellationToken ct) =>
            {
                var users = await userRepository.GetTeamsAsync(id, query, ct);
                return Results.Ok(users.MapItems(t => t.Map()));
            })
            .WithName("GetUserTeams");
        
        group.MapPost("/", async ([FromBody] CreateUserDto user, [FromServices] IUserRepository userRepository, CancellationToken ct) =>
        {
            var createdUser = await userRepository.CreateAsync(user.Map(), ct);
            return Results.Created($"/api/users/{createdUser.Id}", createdUser);
        })
        .WithName("CreateUser");

        group.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateUserDto user, [FromServices] IUserRepository userRepository, CancellationToken ct) =>
        {
            if (id != user.Id)
            {
                return Results.BadRequest("User ID mismatch.");
            }

            var exists = await userRepository.ExistsAsync(id, ct);
            if (!exists)
            {
                return Results.NotFound();
            }

            var updatedUser = await userRepository.UpdateAsync(user.Map(), ct);
            return Results.Ok(updatedUser);
        })
        .WithName("UpdateUser");

        group.MapDelete("/{id:guid}", async (Guid id, [FromServices] IUserRepository userRepository, CancellationToken ct) =>
        {
            var exists = await userRepository.ExistsAsync(id, ct);
            if (!exists)
            {
                return Results.NotFound();
            }

            await userRepository.DeleteAsync(id, ct);
            return Results.NoContent();
        })
        .WithName("DeleteUser");
    }
}