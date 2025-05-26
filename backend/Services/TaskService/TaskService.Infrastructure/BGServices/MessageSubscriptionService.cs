using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Domain;
using TaskService.Domain.Repositories;
using UserService.Domain.PubSub;

namespace TaskService.Infrastructure.BGServices;

public class MessageSubscriptionService : BackgroundService
{
    private readonly IMessageSubscriber _subscriber;
    private readonly ILogger<MessageSubscriptionService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public MessageSubscriptionService(
        IServiceProvider serviceProvider,
        IMessageSubscriber subscriber, 
        ILogger<MessageSubscriptionService> logger)
    {
        _serviceProvider = serviceProvider;
        _subscriber = subscriber;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Registering message subscriptions...");

        await RegisterUserSubscription();
        await RegisterTeamSubscription();
    }

    private async Task RegisterUserSubscription()
    {
        await _subscriber.SubscribeToEvent<PubSubMessage<Shared.Domain.Entities.User>>("task_service_user", async msg =>
        {
            _logger.LogInformation("Received User message: {Message}", JsonSerializer.Serialize(msg));
            await HandleUserMessage(msg);
        });
    }

    private async Task RegisterTeamSubscription()
    {
        await _subscriber.SubscribeToEvent<PubSubMessage<Shared.Domain.Entities.Team>>("task_service_team", async msg =>
        {
            _logger.LogInformation("Received Team message: {Message}", JsonSerializer.Serialize(msg));
            await HandleTeamMessage(msg);
        });
    }

    private async Task HandleUserMessage(PubSubMessage<Shared.Domain.Entities.User> message)
    {
        using var scope = _serviceProvider.CreateScope();
        var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        
        switch (message.Action)
        {
            case MessageAction.Create:
                await userRepo.CreateAsync(message.Data, default);
                break;
            case MessageAction.Update:
                await userRepo.UpdateAsync(message.Data, default);
                break;
            case MessageAction.Delete:
                await userRepo.DeleteAsync(message.Data.Id, default);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(message.Action), message.Action, "Unsupported message action");
        }
    }

    private async Task HandleTeamMessage(PubSubMessage<Shared.Domain.Entities.Team> message)
    {
        using var scope = _serviceProvider.CreateScope();
        var teamRepo = scope.ServiceProvider.GetRequiredService<ITeamRepository>();
        
        switch (message.Action)
        {
            case MessageAction.Create:
                await teamRepo.CreateAsync(message.Data, default);
                break;
            case MessageAction.Update:
                await teamRepo.UpdateAsync(message.Data, default);
                break;
            case MessageAction.Delete:
                await teamRepo.DeleteAsync(message.Data.Id, default);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(message.Action), message.Action, "Unsupported message action");
        }
    }
}