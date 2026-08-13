using System.ComponentModel.DataAnnotations;

namespace MultiShop.Comment.DTOs
{
    public class CreateCommentDto
    {
        [Required]
        [StringLength(64)]
        public string ProductId { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string NameSurname { get; set; } = string.Empty;
        [StringLength(500)]
        public string? ImageUrl { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(2)]
        [StringLength(2000)]
        public string CommentDetail { get; set; } = string.Empty;
        [Range(1,5)]
        public int Rating { get; set; }
    }
}
