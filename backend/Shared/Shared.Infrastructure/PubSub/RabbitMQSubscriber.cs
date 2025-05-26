using EasyNetQ;
using UserService.Domain.PubSub;

namespace Shared.Infrastructure.PubSub;

public class RabbitMQSubscriber: IMessageSubscriber
{
    private readonly IBus _bus;
    
    public RabbitMQSubscriber(IBus bus)
    {
        _bus = bus;
    }
    
    public Task SubscribeToEvent<T>(string subscriptionName, Action<T> onMessage)
    {
        return _bus.PubSub.SubscribeAsync<T>(subscriptionName, onMessage); 
    }
}