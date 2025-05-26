using System.ComponentModel.DataAnnotations;

namespace UserService.Infrastructure.Auth;

public class KeycloakOptions
{
    public const string SectionName = "Keycloak";
    
    [Required(ErrorMessage = "Keycloak URL is required")]
    [MinLength(1, ErrorMessage = "URL cannot be empty")]
    public string Url { get; set; } = string.Empty;

    [Required(ErrorMessage = "Keycloak Realm is required")]
    [MinLength(1, ErrorMessage = "Realm cannot be empty")]
    public string Realm { get; set; } = string.Empty;

    [Required(ErrorMessage = "Keycloak Client ID is required")]
    [MinLength(1, ErrorMessage = "Client ID cannot be empty")]
    public string ClientId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Keycloak Client Secret is required")]
    [MinLength(1, ErrorMessage = "Client Secret cannot be empty")]
    public string ClientSecret { get; set; } = string.Empty;
} 