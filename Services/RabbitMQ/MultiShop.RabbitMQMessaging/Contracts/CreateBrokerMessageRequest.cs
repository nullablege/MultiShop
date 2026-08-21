using System.ComponentModel.DataAnnotations;

namespace MultiShop.RabbitMQMessaging.Contracts;

public sealed class CreateBrokerMessageRequest
{
    [Required]
    [StringLength(500)]
    public string Content { get; init; } = string.Empty;
}
