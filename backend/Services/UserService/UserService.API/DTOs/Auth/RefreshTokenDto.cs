namespace UserService.API.DTOs.Auth;

public class RefreshTokenDto(string token)
{
    public string Token { get; set; } = token;
}