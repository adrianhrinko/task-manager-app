using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure;
using TaskService.Domain.Repositories;
using TaskService.Infrastructure.BGServices;
using TaskService.Infrastructure.Database;
using TaskService.Infrastructure.Repositories;

namespace TaskService.Infrastructure;

public static class DependencyInjection
{
    
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TaskDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("TaskDb"));
        });
        
        services.AddRabbitMqSubscriber(configuration);
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IEpicRepository, EpicRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        
        services.AddHostedService<MessageSubscriptionService>();
        
        return services;
    }
    
}