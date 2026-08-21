using MultiShop.WebUI.Models.Message;

namespace MultiShop.WebUI.Services.MessageServices;

public interface IMessageService
{
    Task<IReadOnlyList<InboxMessageDto>> GetInboxAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SentMessageDto>> GetSentAsync(
        CancellationToken cancellationToken = default);

    Task<bool> MarkAsReadAsync(
        int messageId,
        CancellationToken cancellationToken = default);
}
