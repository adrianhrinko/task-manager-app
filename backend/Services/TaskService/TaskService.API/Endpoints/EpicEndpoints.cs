using Microsoft.AspNetCore.Mvc;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.API.Endpoints;

public static class EpicEndpoints
{
    public static void MapEpicEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("epics");

        // Get epic by ID
        group.MapGet("/{id}", async ([FromRoute] Guid id, IEpicRepository repository) =>
        {
            var epic = await repository.GetByIdAsync(id);
            return epic is null ? Results.NotFound() : Results.Ok(epic);
        });

        // Get epics by project ID
        group.MapGet("/project/{projectId}", async ([FromRoute] Guid projectId, IEpicRepository repository) =>
        {
            var epics = await repository.GetByProjectIdAsync(projectId);
            return Results.Ok(epics);
        });

        // Create new epic
        group.MapPost("/", async ([FromBody] Epic epic, IEpicRepository repository) =>
        {
            await repository.AddAsync(epic);
            return Results.Created($"/epics/{epic.Id}", epic);
        });

        // Update epic
        group.MapPut("/{id}", async ([FromRoute] Guid id, [FromBody] Epic epic, IEpicRepository repository) =>
        {
            if (id != epic.Id)
                return Results.BadRequest();

            try
            {
                await repository.UpdateAsync(epic);
                return Results.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        });

        // Delete epic
        group.MapDelete("/{id}", async ([FromRoute] Guid id, IEpicRepository repository) =>
        {
            if (!await repository.ExistsAsync(id))
                return Results.NotFound();

            await repository.DeleteAsync(id);
            return Results.NoContent();
        });
    }
}