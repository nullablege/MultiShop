namespace MultiShop.WebUI.Models.CommentDTOs
{
    public sealed class AdminCommentListDto
    {
        public int UserCommentId { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public string NameSurname { get; set; } = string.Empty;
        public string CommentDetail { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
    }
}
