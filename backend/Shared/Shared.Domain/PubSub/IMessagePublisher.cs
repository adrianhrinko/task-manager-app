using Shared.Domain;
using Shared.Domain.Entities;

namespace UserService.Domain.PubSub;

public interface IMessagePublisher
{
    Task PublishEvent<T>(PubSubMessage<T> ev, CancellationToken ct);
}