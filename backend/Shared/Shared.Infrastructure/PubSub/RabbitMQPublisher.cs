using EasyNetQ;
using Shared.Domain;
using UserService.Domain.PubSub;

namespace Shared.Infrastructure.PubSub;

public class RabbitMQPublisher : IMessagePublisher
{
    private readonly IBus _bus;
    
    public RabbitMQPublisher(IBus bus)
    {
        _bus = bus;
    }

    public Task PublishEvent<T>(PubSubMessage<T> ev, CancellationToken ct)
    {
        return _bus.PubSub.PublishAsync(ev, ct);
    }
}