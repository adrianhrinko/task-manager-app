namespace UserService.Domain.Entities;

public class UserLogin(string username, string password)
{
    public string Username { get; set; } = username;
    public string Password { get; set; } = password;
}