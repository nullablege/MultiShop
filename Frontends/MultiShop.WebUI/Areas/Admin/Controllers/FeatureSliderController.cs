using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.Catalog.FeatureSliderDTOs;
using MultiShop.WebUI.Services.CatalogServices.FeatureSliderServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class FeatureSliderController : Controller
{
    private readonly IFeatureSliderService _featureSliderService;

    public FeatureSliderController(IFeatureSliderService featureSliderService)
    {
        _featureSliderService = featureSliderService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = await _featureSliderService.GetAllAsync(cancellationToken);
        return View(model);
    }

    public IActionResult CreateFeatureSlider()
    {
        return View(new CreateFeatureSliderDto { IsActive = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateFeatureSlider(CreateFeatureSliderDto createFeatureSliderDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(createFeatureSliderDto);

        await _featureSliderService.CreateAsync(createFeatureSliderDto, cancellationToken);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> UpdateFeatureSlider(string id, CancellationToken cancellationToken)
    {
        var model = await _featureSliderService.GetByIdAsync(id, cancellationToken);
        if (model == null)
            return NotFound();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateFeatureSlider(UpdateFeatureSliderDto updateFeatureSliderDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(updateFeatureSliderDto);

        await _featureSliderService.UpdateAsync(updateFeatureSliderDto, cancellationToken);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFeatureSlider(string featureSliderId, CancellationToken cancellationToken)
    {
        await _featureSliderService.DeleteAsync(featureSliderId, cancellationToken);
        return RedirectToAction("Index");
    }
}
