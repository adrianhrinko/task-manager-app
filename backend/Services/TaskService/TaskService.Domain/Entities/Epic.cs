using TaskService.Domain.Enums;

namespace TaskService.Domain.Entities;

public class Epic(
    Guid id,
    Guid projectId,
    string title,
    DateTime createdAt,
    DateTime updatedAt,
    string? description,
    DateTime startDate,
    DateTime endDate,
    State state = State.New)
{
    public Guid Id { get; set; } = id;
    public string Title { get; set; } = title;
    public string? Description { get; set; } = description;
    public State State { get; set; } = state;
    public Guid ProjectId { get; set; } = projectId;
    public DateTime? StartDate { get; set; } = startDate;
    public DateTime? EndDate { get; set; } = endDate;
    
    public DateTime CreatedAt { get; set; } = createdAt;

    public DateTime UpdatedAt { get; set; } = updatedAt;
}