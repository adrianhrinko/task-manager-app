using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using UserService.Domain.Services;

namespace UserService.Application.Auth;

public class KeycloakAuthService(HttpClient httpClient, IUserRepository userRepository, IConfiguration config, ILogger<KeycloakAuthService> logger) : IAuthService
{
    public async Task<User?> RegisterUserAsync(UserRegistration registration)
    {
        var adminToken = await GetClientAccessTokenAsync();
        if (adminToken == null) return null;

        var keycloakUrl = $"admin/realms/{config["Keycloak:Realm"]}/users";

        var userPayload = new
        {
            username = registration.Username,
            email = registration.Email,
            firstName = registration.FirstName,
            lastName = registration.LastName,
            enabled = true,
            credentials = new[]
            {
                new { type = "password", value = registration.Password, temporary = false }
            }
        };

        var stringContent = JsonSerializer.Serialize(userPayload);
        var content = new StringContent(stringContent, Encoding.UTF8, "application/json");
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);

        var response = await httpClient.PostAsync(keycloakUrl, content);
        
        if (response.IsSuccessStatusCode)
        {
            var now = DateTime.UtcNow;
            return await userRepository.CreateAsync(new UserService.Domain.Entities.User(Guid.Empty, registration.Email, registration.FirstName, registration.LastName, now, now));
        }
        
        return null;
    }

    public async Task<AuthToken?> LoginUserAsync(UserLogin model)
    {
        var keycloakUrl = $"realms/{config["Keycloak:Realm"]}/protocol/openid-connect/token";

        var formData = new Dictionary<string, string>
        {
            { "client_id", config["Keycloak:ClientId"] },
            { "client_secret", config["Keycloak:ClientSecret"] }, 
            { "grant_type", "password" },
            { "username", model.Username },
            { "password", model.Password }
        };

        var content = new FormUrlEncodedContent(formData);
        var response = await httpClient.PostAsync(keycloakUrl, content);
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        var jsonResponse = JsonSerializer.Deserialize<AuthTokenResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return new AuthToken()
        {
            AccessToken = jsonResponse.AccessToken,
            RefreshToken = jsonResponse.RefreshToken,
            ExpiresIn = jsonResponse.ExpiresIn,
            NotBeforePolicy = jsonResponse.NotBeforePolicy,
            RefreshExpiresIn = jsonResponse.RefreshExpiresIn,
            Scope = jsonResponse.Scope, TokenType = jsonResponse.TokenType
        };
    }

    private async Task<string?> GetClientAccessTokenAsync()
    {
        var keycloakUrl = $"realms/{config["Keycloak:Realm"]}/protocol/openid-connect/token";

        var formData = new Dictionary<string, string>
        {
            { "client_id", config["Keycloak:ClientId"] },
            { "client_secret", config["Keycloak:ClientSecret"] },
            { "grant_type", "client_credentials" }
        };

        var content = new FormUrlEncodedContent(formData);
        var response = await httpClient.PostAsync(keycloakUrl, content);
        if (!response.IsSuccessStatusCode) return null;

        try
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<AuthTokenResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return jsonResponse?.AccessToken;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return null;
    }

    public async Task<string?> RefreshTokenAsync(string refreshToken)
    {
        var keycloakUrl = $"realms/{config["Keycloak:Realm"]}/protocol/openid-connect/token";

        var formData = new Dictionary<string, string>
        {
            { "client_id", config["Keycloak:ClientId"] },
            { "client_secret", config["Keycloak:ClientSecret"] },
            { "grant_type", "refresh_token" },
            { "refresh_token", refreshToken }
        };

        var content = new FormUrlEncodedContent(formData);
        var response = await httpClient.PostAsync(keycloakUrl, content);
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        var jsonResponse = JsonSerializer.Deserialize<AuthTokenResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return jsonResponse?.AccessToken;
    }
}

public class AuthTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;
    
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    
    [JsonPropertyName("refresh_expires_in")]
    public int RefreshExpiresIn { get; set; }
    
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = string.Empty;
    
    [JsonPropertyName("not-before-policy")]
    public int NotBeforePolicy { get; set; }
    
    [JsonPropertyName("scope")]
    public string Scope { get; set; } = string.Empty;

    [JsonPropertyName("refresh_token")] 
    public string RefreshToken { get; set; } = string.Empty;
}