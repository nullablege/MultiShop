using MultiShop.RabbitMQMessaging.Contracts;

namespace MultiShop.RabbitMQMessaging.Messaging;

public interface IRabbitMqPublisher
{
    Task PublishAsync(BrokerMessage message, CancellationToken cancellationToken);
}
