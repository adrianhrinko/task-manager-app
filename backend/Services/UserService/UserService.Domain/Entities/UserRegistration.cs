namespace UserService.Domain.Entities;

public class UserRegistration(string username, string email, string firstName, string lastName, string password)
{
    public string Username { get; set; } = username;
    public string Email { get; set; } = email;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Password { get; set; } = password;
}