namespace UserService.Domain.Entities;

public class AuthToken
{
    public string AccessToken { get; set; } = string.Empty;
    
    public int ExpiresIn { get; set; }
    
    public int RefreshExpiresIn { get; set; }
    
    public string TokenType { get; set; } = string.Empty;
    
    public int NotBeforePolicy { get; set; }
    
    public string Scope { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;
}