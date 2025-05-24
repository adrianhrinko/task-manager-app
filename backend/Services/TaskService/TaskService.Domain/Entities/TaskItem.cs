using Shared.Domain;
using TaskService.Domain.Enums;

namespace TaskService.Domain.Entities;

public class TaskItem(
    Guid id,
    Guid epicId,
    Guid projectId,
    string title,
    DateTime createdAt,
    DateTime updatedAt,
    string? description,
    TimeSpan? duration,
    DateTime? scheduledAt,
    DateTime? due,
    State state = State.New,
    User? assignedTo = null)
{
    public Guid Id { get; set; } = id;
    public string Title { get; set; } = title;
    public string? Description { get; set; } = description;
    public State State { get; set; } = state;
    public Guid ProjectId { get; set; } = projectId;
    public Guid EpicId { get; set; } = epicId;
    public TimeSpan? Duration { get; set; } = duration;
    public DateTime? ScheduledAt { get; set; } = scheduledAt;
    public DateTime? Due { get; set; } = due;
    public DateTime CreatedAt { get; set; } = createdAt;

    public DateTime UpdatedAt { get; set; } = updatedAt;

    public User? AssignedTo { get; set; } = assignedTo;
}