namespace UserService.Domain.Entities;

public class Team(Guid id, string name, string description, DateTime createdAt, DateTime updatedAt)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public DateTime CreatedAt { get; set; } = createdAt;
    public DateTime UpdatedAt { get; set; } = updatedAt;
}