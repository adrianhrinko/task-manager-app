using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs.Auth;

public class UserLoginDto(string username, string password)
{
    [Required]
    public string Username { get; } = username;
    [Required]
    public string Password { get; } = password;
}