using Microsoft.AspNetCore.Mvc;
using MultiShop.RabbitMQMessaging.Contracts;
using MultiShop.RabbitMQMessaging.Messaging;

namespace MultiShop.RabbitMQMessaging.Controllers;

[ApiController]
[Route("api/broker-messages")]
public sealed class BrokerMessagesController : ControllerBase
{
    private readonly IRabbitMqPublisher _rabbitMqPublisher;
    private readonly ProcessedMessageStore _processedMessageStore;

    public BrokerMessagesController(
        IRabbitMqPublisher rabbitMqPublisher,
        ProcessedMessageStore processedMessageStore)
    {
        _rabbitMqPublisher = rabbitMqPublisher;
        _processedMessageStore = processedMessageStore;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateBrokerMessageRequest request,
        CancellationToken cancellationToken)
    {
        var message = new BrokerMessage(
            Guid.NewGuid(),
            request.Content.Trim(),
            DateTimeOffset.UtcNow);

        await _rabbitMqPublisher.PublishAsync(message, cancellationToken);

        return Accepted(new { message.Id, message.PublishedAtUtc });
    }

    [HttpGet("processed")]
    public ActionResult<IReadOnlyList<BrokerMessage>> GetProcessed()
    {
        return Ok(_processedMessageStore.GetAll());
    }
}
