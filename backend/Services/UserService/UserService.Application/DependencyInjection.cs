using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Domain.Services;
using UserService.Infrastructure;

namespace UserService.Application;

public static class DependencyInjection
{
    
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddInfrastructureServices(configuration);
        services.AddScoped<IUserService, User.UserService>();
        return services;
    }
    
}