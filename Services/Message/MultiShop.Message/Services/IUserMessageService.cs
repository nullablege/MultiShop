using MultiShop.Message.DTOs;

namespace MultiShop.Message.Services
{
    public interface IUserMessageService
    {
        Task<int> CreateAsync(string senderId, CreateMessageDto createMessageDto, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<InboxMessageDto>> GetInboxAsync(string receiverId, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<SentMessageDto>> GetSentAsync(string senderId, CancellationToken cancellationToken = default);
        Task<bool> MarkAsReadAsync(int messageId, string receiverId, CancellationToken cancellationToken = default);
        Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
    }
}
