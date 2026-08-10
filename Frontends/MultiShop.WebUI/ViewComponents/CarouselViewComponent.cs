using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CatalogServices.FeatureSliderServices;

namespace MultiShop.WebUI.ViewComponents;

public sealed class CarouselViewComponent : ViewComponent
{
    private readonly IFeatureSliderService _featureSliderService;

    public CarouselViewComponent(IFeatureSliderService featureSliderService)
    {
        _featureSliderService = featureSliderService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var featureSliders = await _featureSliderService.GetAllAsync(HttpContext.RequestAborted);
        var activeFeatureSliders = featureSliders
            .Where(featureSlider => featureSlider.IsActive)
            .ToArray();

        return View(activeFeatureSliders);
    }
}
