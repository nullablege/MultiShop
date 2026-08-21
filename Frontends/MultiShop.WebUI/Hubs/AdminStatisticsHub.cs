using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using MultiShop.WebUI.Services.StatisticServices;

namespace MultiShop.WebUI.Hubs;

[Authorize(Roles = "Admin,Manager")]
public sealed class AdminStatisticsHub : Hub
{
    private readonly IAdminStatisticsService _adminStatisticsService;

    public AdminStatisticsHub(IAdminStatisticsService adminStatisticsService)
    {
        _adminStatisticsService = adminStatisticsService;
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("DashboardConnected");
        await base.OnConnectedAsync();
    }

    public async Task RequestDashboardStatisticsAsync()
    {
        var statistics = await _adminStatisticsService.GetAsync(Context.ConnectionAborted);

        await Clients.Caller.SendAsync(
            "DashboardStatisticsUpdated",
            statistics,
            Context.ConnectionAborted);
    }
}
