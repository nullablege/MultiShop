using System.Collections.Concurrent;
using MultiShop.RabbitMQMessaging.Contracts;

namespace MultiShop.RabbitMQMessaging.Messaging;

public sealed class ProcessedMessageStore
{
    private const int Capacity = 20;
    private readonly ConcurrentQueue<BrokerMessage> _messages = new();

    public void Add(BrokerMessage message)
    {
        _messages.Enqueue(message);

        while (_messages.Count > Capacity && _messages.TryDequeue(out _))
        {
        }
    }

    public IReadOnlyList<BrokerMessage> GetAll() => _messages.Reverse().ToList();
}
