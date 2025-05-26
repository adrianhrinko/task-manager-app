using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EasyNetQ;
using Shared.Domain.Exceptions;
using Shared.Infrastructure.PubSub;
using UserService.Domain.PubSub;

namespace Shared.Infrastructure;

public static class DependencyInjectionExtensions
{
    
    public static IServiceCollection AddRabbitMqPublisher(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMessagePublisher, RabbitMQPublisher>();
        services.AddEasyNetQ(configuration.GetConnectionString("RabbitMq") ?? throw new MissingConfigurationException("ConnectionStrings:RabbitMq")).UseSystemTextJson();
        return services;
    }
    
    public static IServiceCollection AddRabbitMqSubscriber(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMessageSubscriber, RabbitMQSubscriber>();
        services.AddEasyNetQ(configuration.GetConnectionString("RabbitMq") ?? throw new MissingConfigurationException("ConnectionStrings:RabbitMq")).UseSystemTextJson();
        return services;
    }
}