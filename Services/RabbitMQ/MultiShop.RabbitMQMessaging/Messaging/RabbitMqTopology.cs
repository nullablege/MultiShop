using RabbitMQ.Client;

namespace MultiShop.RabbitMQMessaging.Messaging;

public sealed class RabbitMqTopology
{
    public const string EventsExchangeName = "multishop.events";
    public const string DeadLetterExchangeName = "multishop.dead-letter";
    public const string MessagesQueueName = "multishop.messages";
    public const string DeadLetterQueueName = "multishop.messages.dead-letter";
    public const string MessagesRoutingKey = "message.created";
    public const string DeadLetterRoutingKey = "message.failed";

    public async Task EnsureCreatedAsync(IChannel channel, CancellationToken cancellationToken)
    {
        await channel.ExchangeDeclareAsync(
            EventsExchangeName,
            ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(
            DeadLetterExchangeName,
            ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            MessagesQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: new Dictionary<string, object?>
            {
                ["x-dead-letter-exchange"] = DeadLetterExchangeName,
                ["x-dead-letter-routing-key"] = DeadLetterRoutingKey
            },
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            MessagesQueueName,
            EventsExchangeName,
            MessagesRoutingKey,
            cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            DeadLetterQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            DeadLetterQueueName,
            DeadLetterExchangeName,
            DeadLetterRoutingKey,
            cancellationToken: cancellationToken);
    }
}
