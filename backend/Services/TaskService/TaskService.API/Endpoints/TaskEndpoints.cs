using Microsoft.AspNetCore.Mvc;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.API.Endpoints;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("tasks");

        // Get task by ID
        group.MapGet("/{id}", async ([FromRoute] Guid id, ITaskRepository repository) =>
        {
            var task = await repository.GetByIdAsync(id);
            return task is null ? Results.NotFound() : Results.Ok(task);
        });

        // Get tasks by epic ID
        group.MapGet("/epic/{epicId}", async ([FromRoute] Guid epicId, ITaskRepository repository) =>
        {
            var tasks = await repository.GetByEpicIdAsync(epicId);
            return Results.Ok(tasks);
        });

        // Get tasks by project ID
        group.MapGet("/project/{projectId}", async ([FromRoute] Guid projectId, ITaskRepository repository) =>
        {
            var tasks = await repository.GetByProjectIdAsync(projectId);
            return Results.Ok(tasks);
        });

        // Create new task
        group.MapPost("/", async ([FromBody] TaskItem task, ITaskRepository repository) =>
        {
            var createdTask = await repository.AddAsync(task);
            return Results.Created($"/tasks/{createdTask.Id}", createdTask);
        });

        // Update existing task
        group.MapPut("/{id}", async ([FromRoute] Guid id, [FromBody] TaskItem task, ITaskRepository repository) =>
        {
            if (id != task.Id)
                return Results.BadRequest("ID mismatch");

            try
            {
                var updatedTask = await repository.UpdateAsync(task);
                return Results.Ok(updatedTask);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        });

        // Delete task
        group.MapDelete("/{id}", async ([FromRoute] Guid id, ITaskRepository repository) =>
        {
            if (!await repository.ExistsAsync(id))
                return Results.NotFound();

            await repository.DeleteAsync(id);
            return Results.NoContent();
        });
    }
}