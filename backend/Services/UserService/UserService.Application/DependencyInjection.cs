using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Exceptions;
using UserService.Application.Auth;
using UserService.Domain;
using UserService.Domain.Services;
using UserService.Infrastructure;

namespace UserService.Application;

public static class DependencyInjection
{
    
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureServices(configuration);
        services.AddHttpClient<IAuthService, KeycloakAuthService>(client =>
        {
            client.BaseAddress =
                new Uri(configuration["Keycloak:Url"] ?? throw new MissingConfigurationException("Keycloak:Url"));
        });
        return services;
    }
    
}