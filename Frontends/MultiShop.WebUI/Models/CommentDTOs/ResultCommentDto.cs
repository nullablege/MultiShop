namespace MultiShop.WebUI.Models.CommentDTOs
{
    public class ResultCommentDto
    {
        public int UserCommentId { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public string NameSurname { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string CommentDetail { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
