using MultiShop.WebUI.Models.StatisticDTOs;

namespace MultiShop.WebUI.Services.StatisticServices;

public interface IAdminStatisticsService
{
    Task<AdminDashboardStatisticsViewModel> GetAsync(
        CancellationToken cancellationToken = default);
}
