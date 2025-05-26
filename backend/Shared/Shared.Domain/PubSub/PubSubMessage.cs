namespace Shared.Domain;

public class PubSubMessage<T>
{
    public T Data { get; set; }

    public MessageAction Action { get; set; }
    
    public PubSubMessage(T data, MessageAction action)
    {
        Data = data;
        Action = action;
    }
}