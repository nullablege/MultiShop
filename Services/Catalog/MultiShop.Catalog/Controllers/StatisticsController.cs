using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Authorization;
using MultiShop.Catalog.DTOs.StatisticDTOs;
using MultiShop.Catalog.Services.StatisticServices;

namespace MultiShop.Catalog.Controllers;

[ApiController]
[Route("api/statistics")]
[Authorize(Policy = CatalogAuthorizationConstants.ManagementPolicy)]
public sealed class StatisticsController : ControllerBase
{
    private readonly ICatalogStatisticsService _catalogStatisticsService;

    public StatisticsController(ICatalogStatisticsService catalogStatisticsService)
    {
        _catalogStatisticsService = catalogStatisticsService;
    }

    [HttpGet]
    public async Task<ActionResult<CatalogStatisticsDto>> GetAsync(
        CancellationToken cancellationToken)
    {
        return Ok(await _catalogStatisticsService.GetAsync(cancellationToken));
    }
}
