using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.StatisticDTOs;
using MultiShop.WebUI.Services.StatisticServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public sealed class DashboardController : Controller
{
    private readonly IAdminStatisticsService _adminStatisticsService;

    public DashboardController(IAdminStatisticsService adminStatisticsService)
    {
        _adminStatisticsService = adminStatisticsService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        try
        {
            return View(await _adminStatisticsService.GetAsync(cancellationToken));
        }
        catch (HttpRequestException)
        {
            ViewData["StatisticsError"] = "İstatistik servislerine şu anda ulaşılamıyor.";
            return View(new AdminDashboardStatisticsViewModel());
        }
    }
}
