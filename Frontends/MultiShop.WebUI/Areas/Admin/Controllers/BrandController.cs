using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.Catalog.BrandDTOs;
using MultiShop.WebUI.Services.CatalogServices.BrandServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class BrandController : Controller
{
    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View(await _brandService.GetAllAsync(cancellationToken));
    }

    public IActionResult CreateBrand()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBrand(CreateBrandDto createBrandDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(createBrandDto);

        await _brandService.CreateAsync(createBrandDto, cancellationToken);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> UpdateBrand(string id, CancellationToken cancellationToken)
    {
        var model = await _brandService.GetByIdAsync(id, cancellationToken);
        if (model == null)
            return NotFound();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateBrand(UpdateBrandDto updateBrandDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(updateBrandDto);

        await _brandService.UpdateAsync(updateBrandDto, cancellationToken);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBrand(string brandId, CancellationToken cancellationToken)
    {
        await _brandService.DeleteAsync(brandId, cancellationToken);
        return RedirectToAction("Index");
    }
}
