using Microsoft.EntityFrameworkCore;
using MultiShop.Message.Context;
using MultiShop.Message.DTOs;
using MultiShop.Message.Entities;

namespace MultiShop.Message.Services
{
    public class UserMessageService : IUserMessageService
    {
        private readonly MessageContext _context;
        public UserMessageService(MessageContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(string senderId, CreateMessageDto createMessageDto, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(senderId))
                throw new ArgumentException(nameof(senderId));

            var message = new UserMessage
            {
                SenderId = senderId,
                ReceiverId = createMessageDto.ReceiverId,
                Subject = createMessageDto.Subject,
                MessageDetail = createMessageDto.MessageDetail,
                IsRead = false,
                CreatedAtUtc = DateTime.UtcNow,
            };
            var userMessage = await _context.UserMessages.AddAsync(message, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return userMessage.Entity.UserMessageId;
        }

        public async Task<IReadOnlyList<InboxMessageDto>> GetInboxAsync(string receiverId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(receiverId))
                throw new ArgumentException(nameof(receiverId));

            var inbox = await _context.UserMessages.Where(x => x.ReceiverId == receiverId)
                .OrderByDescending(x => x.CreatedAtUtc)
                .Select(x => new InboxMessageDto
                {
                    UserMessageId = x.UserMessageId,
                    SenderId = x.SenderId,
                    Subject = x.Subject,
                    MessageDetail = x.MessageDetail,
                    IsRead = x.IsRead,
                    CreatedAtUtc = x.CreatedAtUtc
                }).AsNoTracking().ToListAsync(cancellationToken);

            return inbox;
        }

        public async Task<IReadOnlyList<SentMessageDto>> GetSentAsync(string senderId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(senderId))
                throw new ArgumentException(nameof(senderId));

            var sent = await _context.UserMessages.Where(x => x.SenderId == senderId)
                .OrderByDescending(x => x.CreatedAtUtc)
                .Select(x => new SentMessageDto
                {
                    UserMessageId = x.UserMessageId,
                    ReceiverId = x.ReceiverId,
                    Subject = x.Subject,
                    MessageDetail = x.MessageDetail,
                    IsRead = x.IsRead,
                    CreatedAtUtc = x.CreatedAtUtc
                }).AsNoTracking().ToListAsync(cancellationToken);

            return sent;
        }

        public async Task<bool> MarkAsReadAsync(int messageId, string receiverId, CancellationToken cancellationToken = default)
        {
            if (messageId <= 0)
                throw new ArgumentOutOfRangeException(nameof(messageId));
            if (string.IsNullOrWhiteSpace(receiverId))
                throw new ArgumentException(nameof(receiverId));

            var message = await _context.UserMessages.Where(x => x.UserMessageId == messageId && x.ReceiverId == receiverId).SingleOrDefaultAsync(cancellationToken);

            if (message == null)
                return false;

            if (!message.IsRead)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            return true;
        }

        public Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
        {
            return _context.UserMessages.CountAsync(cancellationToken);
        }
    }
}
