using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Database;
using UserService.Infrastructure.Repositories;
using Shared.Infrastructure;
using UserService.Domain.Clients;
using UserService.Infrastructure.Auth;

namespace UserService.Infrastructure;

public static class DependencyInjection
{
    
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("UserDb"));
        });

        services.AddOptions<KeycloakOptions>()
            .Bind(configuration.GetSection(KeycloakOptions.SectionName))
            .ValidateDataAnnotations();        
            
        services.AddHttpClient<IAuthProviderClient, KeycloakAuthProviderClient>((serviceProvider, client) =>
        {
            var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;
            client.BaseAddress = new Uri(keycloakOptions.Url.TrimEnd('/') + "/");
        });

        services.AddRabbitMqPublisher(configuration);
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();

        return services;
    }
    
}