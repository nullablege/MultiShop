using MultiShop.WebUI.Models.StatisticDTOs;

namespace MultiShop.WebUI.Services.StatisticServices;

public sealed class AdminStatisticsService : IAdminStatisticsService
{
    public const string IdentityClientName = "AdminStatistics.Identity";
    public const string CatalogClientName = "AdminStatistics.Catalog";
    public const string CommentClientName = "AdminStatistics.Comment";
    public const string DiscountClientName = "AdminStatistics.Discount";
    public const string MessageClientName = "AdminStatistics.Message";

    private readonly IHttpClientFactory _httpClientFactory;

    public AdminStatisticsService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<AdminDashboardStatisticsViewModel> GetAsync(
        CancellationToken cancellationToken = default)
    {
        var catalogTask = GetRequiredAsync<CatalogStatisticsDto>(CatalogClientName, "statistics", cancellationToken);
        var userTask = GetRequiredAsync<UserCountDto>(IdentityClientName, "api/users/count", cancellationToken);
        var commentTask = GetRequiredAsync<CommentStatisticsDto>(CommentClientName, "comments/admin/statistics", cancellationToken);
        var discountTask = GetRequiredAsync<DiscountStatisticsDto>(DiscountClientName, "discounts/count", cancellationToken);
        var messageTask = GetRequiredAsync<MessageStatisticsDto>(MessageClientName, "messages/admin/statistics", cancellationToken);

        await Task.WhenAll(catalogTask, userTask, commentTask, discountTask, messageTask);

        var catalog = await catalogTask;
        var users = await userTask;
        var comments = await commentTask;
        var discounts = await discountTask;
        var messages = await messageTask;

        return new AdminDashboardStatisticsViewModel
        {
            BrandCount = catalog.BrandCount,
            CategoryCount = catalog.CategoryCount,
            ProductCount = catalog.ProductCount,
            AverageProductPrice = catalog.AverageProductPrice,
            MostExpensiveProductName = catalog.MostExpensiveProductName,
            LeastExpensiveProductName = catalog.LeastExpensiveProductName,
            UserCount = users.Count,
            TotalCommentCount = comments.TotalCount,
            ApprovedCommentCount = comments.ApprovedCount,
            PendingCommentCount = comments.PendingCount,
            DiscountCouponCount = discounts.Count,
            TotalMessageCount = messages.TotalCount
        };
    }

    private async Task<T> GetRequiredAsync<T>(
        string clientName,
        string requestUri,
        CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient(clientName);
        var result = await client.GetFromJsonAsync<T>(requestUri, cancellationToken);
        return result ?? throw new InvalidOperationException("İstatistik yanıtı boş döndü.");
    }

    private sealed class CatalogStatisticsDto { public long BrandCount { get; init; } public long CategoryCount { get; init; } public long ProductCount { get; init; } public decimal AverageProductPrice { get; init; } public string MostExpensiveProductName { get; init; } = string.Empty; public string LeastExpensiveProductName { get; init; } = string.Empty; }
    private sealed class UserCountDto { public int Count { get; init; } }
    private sealed class CommentStatisticsDto { public int TotalCount { get; init; } public int ApprovedCount { get; init; } public int PendingCount { get; init; } }
    private sealed class DiscountStatisticsDto { public int Count { get; init; } }
    private sealed class MessageStatisticsDto { public int TotalCount { get; init; } }
}
