using MultiShop.Catalog.DTOs.StatisticDTOs;

namespace MultiShop.Catalog.Services.StatisticServices;

public interface ICatalogStatisticsService
{
    Task<CatalogStatisticsDto> GetAsync(CancellationToken cancellationToken = default);
}
