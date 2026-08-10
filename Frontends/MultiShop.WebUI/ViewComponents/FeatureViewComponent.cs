using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CatalogServices.FeatureServices;

namespace MultiShop.WebUI.ViewComponents;

public sealed class FeatureViewComponent : ViewComponent
{
    private readonly IFeatureService _featureService;

    public FeatureViewComponent(IFeatureService featureService)
    {
        _featureService = featureService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _featureService.GetAllAsync(HttpContext.RequestAborted));
    }
}
