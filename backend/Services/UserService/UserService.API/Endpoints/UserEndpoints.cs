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
        
        group.MapGet("/{id:guid}", async (Guid id, [FromServices] IUserRepository userRepository) =>
        {
            var user = await userRepository.GetByIdAsync(id);
            return user is not null ? Results.Ok(user.Map()) : Results.NotFound();
        })
        .WithName("GetUserById");

        group.MapGet("/by-email/{email}", async (string email, [FromServices] IUserRepository userRepository) =>
        {
            var user = await userRepository.GetByEmailAsync(email);
            return user is not null ? Results.Ok(user.Map()) : Results.NotFound();
        })
        .WithName("GetUserByEmail");

        group.MapGet("/", async ([AsParameters] Query query, [FromServices] IUserRepository userRepository) =>
            {
                var users = await userRepository.GetAllAsync(query);
                return Results.Ok(users.MapItems(u => u.Map()));
            })
            .WithName("GetAllUsers");
        
        group.MapGet("/{id:guid}/teams", async (Guid id, [AsParameters] Query query, [FromServices] IUserRepository userRepository) =>
            {
                var users = await userRepository.GetTeamsAsync(id, query);
                return Results.Ok(users.MapItems(t => t.Map()));
            })
            .WithName("GetUserTeams");
        
        group.MapPost("/", async ([FromBody] CreateUserDto user, [FromServices] IUserRepository userRepository) =>
        {
            var createdUser = await userRepository.CreateAsync(user.Map());
            return Results.Created($"/api/users/{createdUser.Id}", createdUser);
        })
        .WithName("CreateUser");

        group.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateUserDto user, [FromServices] IUserRepository userRepository) =>
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

            var updatedUser = await userRepository.UpdateAsync(user.Map());
            return Results.Ok(updatedUser);
        })
        .WithName("UpdateUser");

        group.MapDelete("/{id:guid}", async (Guid id, [FromServices] IUserRepository userRepository) =>
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
    }
}