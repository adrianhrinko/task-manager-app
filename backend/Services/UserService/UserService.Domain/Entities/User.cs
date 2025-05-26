namespace UserService.Domain.Entities;

public class User(Guid id, string email, string firstName, string lastName, DateTime createdAt, DateTime updatedAt) 
    : Shared.Domain.Entities.User(id, email, firstName, lastName)
{
    public DateTime CreatedAt { get; set; } = createdAt;
    public DateTime UpdatedAt { get; set; } = updatedAt;
}
