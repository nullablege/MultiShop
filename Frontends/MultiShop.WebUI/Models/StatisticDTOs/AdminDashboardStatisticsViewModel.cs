namespace MultiShop.WebUI.Models.StatisticDTOs;

public sealed class AdminDashboardStatisticsViewModel
{
    public long BrandCount { get; init; }
    public long CategoryCount { get; init; }
    public long ProductCount { get; init; }
    public decimal AverageProductPrice { get; init; }
    public string MostExpensiveProductName { get; init; } = string.Empty;
    public string LeastExpensiveProductName { get; init; } = string.Empty;
    public int UserCount { get; init; }
    public int TotalCommentCount { get; init; }
    public int ApprovedCommentCount { get; init; }
    public int PendingCommentCount { get; init; }
    public int DiscountCouponCount { get; init; }
    public int TotalMessageCount { get; init; }
}
