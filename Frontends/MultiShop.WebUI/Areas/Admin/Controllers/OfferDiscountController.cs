using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.Catalog.OfferDiscountDTOs;
using MultiShop.WebUI.Services.CatalogServices.OfferDiscountServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class OfferDiscountController : Controller
{
    private readonly IOfferDiscountService _offerDiscountService;

    public OfferDiscountController(IOfferDiscountService offerDiscountService)
    {
        _offerDiscountService = offerDiscountService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View(await _offerDiscountService.GetAllAsync(cancellationToken));
    }

    public IActionResult CreateOfferDiscount()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOfferDiscount(CreateOfferDiscountDto createOfferDiscountDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(createOfferDiscountDto);

        await _offerDiscountService.CreateAsync(createOfferDiscountDto, cancellationToken);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> UpdateOfferDiscount(string id, CancellationToken cancellationToken)
    {
        var model = await _offerDiscountService.GetByIdAsync(id, cancellationToken);
        if (model == null)
            return NotFound();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateOfferDiscount(UpdateOfferDiscountDto updateOfferDiscountDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(updateOfferDiscountDto);

        await _offerDiscountService.UpdateAsync(updateOfferDiscountDto, cancellationToken);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteOfferDiscount(string offerDiscountId, CancellationToken cancellationToken)
    {
        await _offerDiscountService.DeleteAsync(offerDiscountId, cancellationToken);
        return RedirectToAction("Index");
    }
}
