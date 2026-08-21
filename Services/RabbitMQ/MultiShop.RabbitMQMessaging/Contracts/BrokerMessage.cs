namespace MultiShop.RabbitMQMessaging.Contracts;

public sealed record BrokerMessage(
    Guid Id,
    string Content,
    DateTimeOffset PublishedAtUtc);
