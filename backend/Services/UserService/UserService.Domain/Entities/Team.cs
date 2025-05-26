namespace UserService.Domain.Entities;

public class Team(Guid id, string name, string description, DateTime createdAt, DateTime updatedAt)
    : Shared.Domain.Entities.Team(id, name)
{ public string Description { get; set; } = description;
    public DateTime CreatedAt { get; set; } = createdAt;
    public DateTime UpdatedAt { get; set; } = updatedAt;
}