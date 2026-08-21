namespace MultiShop.Catalog.DTOs.StatisticDTOs;

public sealed class CatalogStatisticsDto
{
    public long BrandCount { get; init; }
    public long CategoryCount { get; init; }
    public long ProductCount { get; init; }
    public decimal AverageProductPrice { get; init; }
    public string MostExpensiveProductName { get; init; } = string.Empty;
    public string LeastExpensiveProductName { get; init; } = string.Empty;
}
