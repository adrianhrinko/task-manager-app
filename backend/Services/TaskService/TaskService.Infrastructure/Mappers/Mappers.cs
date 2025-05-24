using TaskService.Domain.Entities;
using TaskService.Infrastructure.Database.Entities;

namespace TaskService.Infrastructure.Mappers;

public static class Mappers
{
    public static Database.Entities.Project Map(this Domain.Entities.Project project)
        => new()
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                State = project.State,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                TeamId = project.TeamId,
                OwnerId = project.OwnerId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

    public static Domain.Entities.Project Map(this Database.Entities.Project project)
        => new(
            id: project.Id,
            teamId: project.TeamId,
            ownerId: project.OwnerId,
            title: project.Title,
            description: project.Description,
            createdAt: project.CreatedAt,
            updatedAt: project.UpdatedAt,
            startDate: project.StartDate,
            endDate: project.EndDate,
            state: project.State
        );

    public static Database.Entities.TaskItem Map(this Domain.Entities.TaskItem task)
        => new()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                State = task.State,
                ProjectId = task.ProjectId,
                EpicId = task.EpicId,
                Duration = task.Duration,
                ScheduledAt = task.ScheduledAt,
                Due = task.Due,
                AssignedTo = task.AssignedTo?.Map(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

    public static Domain.Entities.TaskItem Map(this Database.Entities.TaskItem task)
        => new(
            id: task.Id,
            epicId: task.EpicId,
            projectId: task.ProjectId,
            title: task.Title,
            description: task.Description,
            createdAt: task.CreatedAt,
            updatedAt: task.UpdatedAt,
            duration: task.Duration,
            scheduledAt: task.ScheduledAt,
            due: task.Due,
            state: task.State,
            assignedTo: task.AssignedTo?.Map() 
        );

    public static Database.Entities.Epic Map(this Domain.Entities.Epic epic)
        => new()
            {
                Id = epic.Id,
                Title = epic.Title,
                Description = epic.Description,
                State = epic.State,
                ProjectId = epic.ProjectId,
                StartDate = epic.StartDate,
                EndDate = epic.EndDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

    public static Domain.Entities.Epic Map(this Database.Entities.Epic epic)
        => new(
            id: epic.Id,
            projectId: epic.ProjectId,
            title: epic.Title,
            description: epic.Description,
            createdAt: epic.CreatedAt,
            updatedAt: epic.CreatedAt,
            startDate: epic.StartDate ?? DateTime.UtcNow, 
            endDate: epic.EndDate ?? DateTime.UtcNow, 
            state: epic.State
        );
    
    public static User Map(this Shared.Domain.User user)
        => new()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

    public static Shared.Domain.User Map(this User user)
        => new(
            id: user.Id,
            email: user.Email,
            firstName: user.FirstName,
            lastName: user.LastName
        );
}