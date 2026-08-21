namespace MultiShop.Comment.DTOs;

public sealed class CommentStatisticsDto
{
    public int TotalCount { get; init; }
    public int ApprovedCount { get; init; }
    public int PendingCount { get; init; }
}
