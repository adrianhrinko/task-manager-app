using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserService.Domain.Clients;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;

namespace UserService.Infrastructure.Auth;

public class KeycloakAuthProviderClient : IAuthProviderClient
{
    private readonly HttpClient _httpClient;
    private readonly IUserRepository _userRepository;
    private readonly KeycloakOptions _keycloakOptions;
    private readonly ILogger<KeycloakAuthProviderClient> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public KeycloakAuthProviderClient(
        HttpClient httpClient,
        IUserRepository userRepository,
        IOptions<KeycloakOptions> keycloakOptions,
        ILogger<KeycloakAuthProviderClient> logger)
    {
        _httpClient = httpClient;
        _userRepository = userRepository;
        _keycloakOptions = keycloakOptions.Value;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    private static class Endpoints
    {
        public static string AdminUsers(string realm) => $"admin/realms/{realm}/users";
        public static string Token(string realm) => $"realms/{realm}/protocol/openid-connect/token";
    }
    
    public async Task<RegistrationResult> RegisterUserAsync(UserRegistration registration, CancellationToken ct)
    {
        var adminToken = await GetClientAccessTokenAsync(ct);
        if (adminToken == null)
        {
            return RegistrationResult.Failure;
        }

        var url = Endpoints.AdminUsers(_keycloakOptions.Realm);

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
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);

        var response = await _httpClient.PostAsync(url, content, ct);
            
        if (response.IsSuccessStatusCode)
        {
            return RegistrationResult.Success;
        }

        var errorContent = await response.Content.ReadAsStringAsync(ct);
        _logger.LogWarning("User registration failed. Status: {StatusCode}, Error: {Error}", 
            response.StatusCode, errorContent);
                
        return response.StatusCode is HttpStatusCode.Conflict ? RegistrationResult.Conflict : RegistrationResult.Failure;
    }

    public async Task<AuthToken?> LoginUserAsync(UserLogin model, CancellationToken ct)
    {
        var url = Endpoints.Token(_keycloakOptions.Realm);

        var formData = new Dictionary<string, string>
        {
            { "client_id", _keycloakOptions.ClientId },
            { "client_secret", _keycloakOptions.ClientSecret }, 
            { "grant_type", "password" },
            { "username", model.Username },
            { "password", model.Password }
        };

        var content = new FormUrlEncodedContent(formData);
        var response = await _httpClient.PostAsync(url, content, ct);
        
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Failed to login user. Status code: {StatusCode}", response.StatusCode);
            return null;
        }

        var responseBody = await response.Content.ReadAsStringAsync(ct);
        var jsonResponse = JsonSerializer.Deserialize<AuthTokenResponse>(responseBody, _jsonOptions);

        if (jsonResponse is null)
        {
            _logger.LogError("Failed to deserialize auth token response");
            return null;
        }
        
        return MapToAuthToken(jsonResponse);
    }

    private async Task<string?> GetClientAccessTokenAsync(CancellationToken ct)
    {
        var url = Endpoints.Token(_keycloakOptions.Realm);

        var formData = new Dictionary<string, string>
        {
            { "client_id", _keycloakOptions.ClientId },
            { "client_secret", _keycloakOptions.ClientSecret },
            { "grant_type", "client_credentials" }
        };

        var content = new FormUrlEncodedContent(formData);
        var response = await _httpClient.PostAsync(url, content, ct);
        
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Failed to get client access token. Status code: {StatusCode}", response.StatusCode);
            return null;
        }

        try
        {
            var responseBody = await response.Content.ReadAsStringAsync(ct);
            var jsonResponse = JsonSerializer.Deserialize<AuthTokenResponse>(responseBody, _jsonOptions);
            return jsonResponse?.AccessToken;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to get client access token");
        }

        return null;
    }

    public async Task<AuthToken?> RefreshTokenAsync(string refreshToken, CancellationToken ct)
    {
        var url = Endpoints.Token(_keycloakOptions.Realm);

        var formData = new Dictionary<string, string>
        {
            { "client_id", _keycloakOptions.ClientId },
            { "client_secret", _keycloakOptions.ClientSecret },
            { "grant_type", "refresh_token" },
            { "refresh_token", refreshToken }
        };

        var content = new FormUrlEncodedContent(formData);
        var response = await _httpClient.PostAsync(url, content, ct);
        
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Failed to refresh token. Status code: {StatusCode}", response.StatusCode);
            return null;
        }

        var responseBody = await response.Content.ReadAsStringAsync(ct);
        var jsonResponse = JsonSerializer.Deserialize<AuthTokenResponse>(responseBody, _jsonOptions);
        
        if (jsonResponse is null)
        {
            _logger.LogError("Failed to deserialize auth token response");
            return null;
        }
        
        return MapToAuthToken(jsonResponse);
    }

    private static AuthToken MapToAuthToken(AuthTokenResponse response) => new()
    {
        AccessToken = response.AccessToken,
        RefreshToken = response.RefreshToken,
        ExpiresIn = response.ExpiresIn,
        NotBeforePolicy = response.NotBeforePolicy,
        RefreshExpiresIn = response.RefreshExpiresIn,
        Scope = response.Scope,
        TokenType = response.TokenType
    };
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