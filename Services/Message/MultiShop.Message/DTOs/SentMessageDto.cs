namespace MultiShop.Message.DTOs
{
    public class SentMessageDto
    {
        public int UserMessageId { get; set; }
        public string ReceiverId { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string MessageDetail { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
