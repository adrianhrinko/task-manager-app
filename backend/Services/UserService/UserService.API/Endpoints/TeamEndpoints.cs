using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Domain;
using UserService.API.DTOs;
using UserService.API.DTOs.Teams;
using UserService.API.Mappers;
using UserService.Domain.Repositories;

namespace UserService.API.Endpoints;

public static class TeamEndpoints
{
    public static void MapTeamEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("teams");
        
        group.MapGet("/{id:guid}", async (Guid id, [FromServices] ITeamRepository teamRepository) =>
        {
            var team = await teamRepository.GetByIdAsync(id);
            return team is not null ? Results.Ok(team) : Results.NotFound();
        })
        .WithName("GetTeamById");

        group.MapGet("/", async ([AsParameters] Query query, [FromServices] ITeamRepository teamRepository) =>
        {
            var teams = await teamRepository.GetAllAsync(query);
            return Results.Ok(teams.MapItems(t => t.Map()));
        })
        .WithName("GetAllTeams");
        
        group.MapGet("/{id:guid}/members", async (Guid id, [AsParameters] Query query, [FromServices] ITeamRepository teamRepository) =>
            {
                var teamMembers = await teamRepository.GetTeamMembersAsync(id, query);
                return Results.Ok(teamMembers.MapItems(t => t.Map()));
            })
        .WithName("GetTeamMembers");
    
        group.MapPost("/", async ([FromBody] CreateTeamDto team, [FromServices] ITeamRepository teamRepository) =>
        {
            var createdTeam = await teamRepository.CreateAsync(team.CreatedBy, team.Map());
            return Results.Created($"/api/teams/{createdTeam.Id}", createdTeam);
        })
        .WithName("CreateTeam");

        group.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateTeamDto team, [FromServices] ITeamRepository teamRepository) =>
        {
            if (id != team.Id)
            {
                return Results.BadRequest("Team ID mismatch.");
            }
            
            var exists = await teamRepository.ExistsAsync(id);
            if (!exists)
            {

                return Results.NotFound();
            }

            var updatedTeam = await teamRepository.UpdateAsync(team.Map());
            return Results.Ok(updatedTeam);
        })
        .WithName("UpdateTeam");

        group.MapDelete("/{id:guid}", async (Guid id, [FromServices] ITeamRepository teamRepository) =>
        {
            var exists = await teamRepository.ExistsAsync(id);
            if (!exists)
            {
                return Results.NotFound();
            }

            await teamRepository.DeleteAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteTeam");
    }
} 