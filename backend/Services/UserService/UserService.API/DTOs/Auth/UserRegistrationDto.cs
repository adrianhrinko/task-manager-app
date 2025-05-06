using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs.Auth;

public class UserRegistrationDto(string username, string email, string firstName, string lastName, string password)
{
    [Required]
    public string Username { get; } = username;
    [EmailAddress]
    public string Email { get; } = email;
    [Required]
    public string FirstName { get; } = firstName;
    [Required]
    public string LastName { get; } = lastName;
    [Required]
    public string Password { get; } = password;
}