using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.Catalog.SpecialOfferDTOs;
using MultiShop.WebUI.Services.CatalogServices.SpecialOfferServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class SpecialOfferController : Controller
{
    private readonly ISpecialOfferService _specialOfferService;

    public SpecialOfferController(ISpecialOfferService specialOfferService)
    {
        _specialOfferService = specialOfferService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = await _specialOfferService.GetAllAsync(cancellationToken);
        return View(model);
    }

    public IActionResult CreateSpecialOffer()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateSpecialOffer(CreateSpecialOfferDto createSpecialOfferDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(createSpecialOfferDto);

        await _specialOfferService.CreateAsync(createSpecialOfferDto, cancellationToken);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> UpdateSpecialOffer(string id, CancellationToken cancellationToken)
    {
        var model = await _specialOfferService.GetByIdAsync(id, cancellationToken);
        if (model == null)
            return NotFound();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateSpecialOffer(UpdateSpecialOfferDto updateSpecialOfferDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(updateSpecialOfferDto);

        await _specialOfferService.UpdateAsync(updateSpecialOfferDto, cancellationToken);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteSpecialOffer(string specialOfferId, CancellationToken cancellationToken)
    {
        await _specialOfferService.DeleteAsync(specialOfferId, cancellationToken);
        return RedirectToAction("Index");
    }
}
