using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.Catalog.FeatureDTOs;
using MultiShop.WebUI.Services.CatalogServices.FeatureServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class FeatureController : Controller
{
    private readonly IFeatureService _featureService;

    public FeatureController(IFeatureService featureService)
    {
        _featureService = featureService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View(await _featureService.GetAllAsync(cancellationToken));
    }

    public IActionResult CreateFeature()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateFeature(CreateFeatureDto createFeatureDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(createFeatureDto);

        await _featureService.CreateAsync(createFeatureDto, cancellationToken);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> UpdateFeature(string id, CancellationToken cancellationToken)
    {
        var model = await _featureService.GetByIdAsync(id, cancellationToken);
        if (model == null)
            return NotFound();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateFeature(UpdateFeatureDto updateFeatureDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(updateFeatureDto);

        await _featureService.UpdateAsync(updateFeatureDto, cancellationToken);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFeature(string featureId, CancellationToken cancellationToken)
    {
        await _featureService.DeleteAsync(featureId, cancellationToken);
        return RedirectToAction("Index");
    }
}
