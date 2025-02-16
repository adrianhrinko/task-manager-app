namespace UserService.Domain.Entities;

public class User(Guid id, string email, string firstName, string lastName, DateTime createdAt, DateTime updatedAt) 
    : UserBase(id, email, firstName, lastName)
{
    public DateTime CreatedAt { get; set; } = createdAt;
    public DateTime UpdatedAt { get; set; } = updatedAt;
}

public class UserBase(Guid id, string email, string firstName, string lastName)
{
    public Guid Id { get; set; } = id;
    public string Email { get; set; } = email;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
}
