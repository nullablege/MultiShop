using System.ComponentModel.DataAnnotations;

namespace MultiShop.Message.DTOs
{
    public class CreateMessageDto
    {
        [Required]
        [MaxLength(450)]
        public string ReceiverId { get; set; } = string.Empty;
        [Required]
        [MaxLength(200)]
        public string Subject { get; set; } = string.Empty;
        [Required]
        [MaxLength(4000)]
        public string MessageDetail {  get; set; } = string.Empty;
    }
}
