using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.API.Endpoints;

public static class ProjectEndpoints
{
    public static void MapProjectEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("projects");

        group.MapGet("/{id}", async (Guid id, IProjectRepository repository) =>
        {
            var project = await repository.GetByIdAsync(id);
            return project is null ? Results.NotFound() : Results.Ok(project);
        });

        group.MapGet("/owner/{ownerId}", async (Guid ownerId, IProjectRepository repository) =>
        {
            var projects = await repository.GetByOwnerId(ownerId);
            return Results.Ok(projects);
        });

        group.MapGet("/team/{teamId}", async (Guid teamId, IProjectRepository repository) =>
        {
            var projects = await repository.GetByTeamId(teamId);
            return Results.Ok(projects);
        });

        group.MapPost("/", async (Project project, IProjectRepository repository) =>
        {
            var created = await repository.CreateAsync(project);
            return Results.Created($"/projects/{created.Id}", created);
        });

        group.MapPut("/{id}", async (Guid id, Project project, IProjectRepository repository) =>
        {
            if (id != project.Id)
                return Results.BadRequest("ID mismatch");

            try
            {
                var updated = await repository.UpdateAsync(project);
                return Results.Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        });

        group.MapDelete("/{id}", async (Guid id, IProjectRepository repository) =>
        {
            if (!await repository.ExistsAsync(id))
                return Results.NotFound();

            await repository.DeleteAsync(id);
            return Results.NoContent();
        });
    }
}