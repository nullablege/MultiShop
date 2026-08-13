namespace MultiShop.Comment.Entities
{
    public class UserComment
    {
        public int UserCommentId { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public string NameSurname { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string Email { get; set; } = string.Empty;
        public string CommentDetail { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
    }
}
