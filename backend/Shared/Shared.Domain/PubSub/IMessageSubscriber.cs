using Shared.Domain;

namespace UserService.Domain.PubSub;

public interface IMessageSubscriber
{
    public Task SubscribeToEvent<T>(string subscriptionName, Action<T> onMessage);
}