using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CatalogServices.SpecialOfferServices;

namespace MultiShop.WebUI.ViewComponents;

public sealed class SpecialOfferViewComponent : ViewComponent
{
    private readonly ISpecialOfferService _specialOfferService;

    public SpecialOfferViewComponent(ISpecialOfferService specialOfferService)
    {
        _specialOfferService = specialOfferService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var specialOffers = await _specialOfferService.GetAllAsync(HttpContext.RequestAborted);
        return View(specialOffers);
    }
}
