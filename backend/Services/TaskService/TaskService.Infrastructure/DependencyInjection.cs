using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskService.Domain.Repositories;
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

        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IEpicRepository, EpicRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();

        return services;
    }
    
}