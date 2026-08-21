using System.Text.Json;
using MultiShop.RabbitMQMessaging.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MultiShop.RabbitMQMessaging.Messaging;

public sealed class RabbitMqConsumer : BackgroundService
{
    private readonly RabbitMqConnection _rabbitMqConnection;
    private readonly RabbitMqTopology _rabbitMqTopology;
    private readonly ProcessedMessageStore _processedMessageStore;
    private readonly ILogger<RabbitMqConsumer> _logger;

    public RabbitMqConsumer(
        RabbitMqConnection rabbitMqConnection,
        RabbitMqTopology rabbitMqTopology,
        ProcessedMessageStore processedMessageStore,
        ILogger<RabbitMqConsumer> logger)
    {
        _rabbitMqConnection = rabbitMqConnection;
        _rabbitMqTopology = rabbitMqTopology;
        _processedMessageStore = processedMessageStore;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var connection = await _rabbitMqConnection.GetOpenConnectionAsync(stoppingToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _rabbitMqTopology.EnsureCreatedAsync(channel, stoppingToken);
        await channel.BasicQosAsync(
            prefetchSize: 0,
            prefetchCount: 1,
            global: false,
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, eventArgs) =>
        {
            try
            {
                var message = JsonSerializer.Deserialize<BrokerMessage>(eventArgs.Body.Span)
                    ?? throw new JsonException("RabbitMQ mesaj içeriği okunamadı.");

                _processedMessageStore.Add(message);
                _logger.LogInformation(
                    "RabbitMQ mesajı işlendi. MesajId: {MessageId}",
                    message.Id);

                await channel.BasicAckAsync(
                    eventArgs.DeliveryTag,
                    multiple: false,
                    cancellationToken: CancellationToken.None);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "RabbitMQ mesajı işlenemedi ve dead-letter queue'ya taşındı.");

                await channel.BasicNackAsync(
                    eventArgs.DeliveryTag,
                    multiple: false,
                    requeue: false,
                    cancellationToken: CancellationToken.None);
            }
        };

        var consumerTag = await channel.BasicConsumeAsync(
            RabbitMqTopology.MessagesQueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        _logger.LogInformation("RabbitMQ consumer başlatıldı. Queue: {Queue}", RabbitMqTopology.MessagesQueueName);

        try
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            await channel.BasicCancelAsync(
                consumerTag,
                noWait: false,
                cancellationToken: CancellationToken.None);
        }
    }
}
