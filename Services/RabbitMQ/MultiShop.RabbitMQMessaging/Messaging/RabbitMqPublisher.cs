using System.Text.Json;
using MultiShop.RabbitMQMessaging.Contracts;
using RabbitMQ.Client;

namespace MultiShop.RabbitMQMessaging.Messaging;

public sealed class RabbitMqPublisher : IRabbitMqPublisher
{
    private readonly RabbitMqConnection _rabbitMqConnection;
    private readonly RabbitMqTopology _rabbitMqTopology;

    public RabbitMqPublisher(
        RabbitMqConnection rabbitMqConnection,
        RabbitMqTopology rabbitMqTopology)
    {
        _rabbitMqConnection = rabbitMqConnection;
        _rabbitMqTopology = rabbitMqTopology;
    }

    public async Task PublishAsync(BrokerMessage message, CancellationToken cancellationToken)
    {
        var connection = await _rabbitMqConnection.GetOpenConnectionAsync(cancellationToken);

        await using var channel = await connection.CreateChannelAsync(
            new CreateChannelOptions(
                publisherConfirmationsEnabled: true,
                publisherConfirmationTrackingEnabled: true),
            cancellationToken);

        await _rabbitMqTopology.EnsureCreatedAsync(channel, cancellationToken);

        var body = JsonSerializer.SerializeToUtf8Bytes(message);
        var properties = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent,
            MessageId = message.Id.ToString(),
            Type = nameof(BrokerMessage)
        };

        await channel.BasicPublishAsync(
            RabbitMqTopology.EventsExchangeName,
            RabbitMqTopology.MessagesRoutingKey,
            mandatory: true,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);
    }
}
