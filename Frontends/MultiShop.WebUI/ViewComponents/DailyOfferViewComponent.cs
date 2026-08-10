using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CatalogServices.OfferDiscountServices;

namespace MultiShop.WebUI.ViewComponents;

public sealed class DailyOfferViewComponent : ViewComponent
{
    private readonly IOfferDiscountService _offerDiscountService;

    public DailyOfferViewComponent(IOfferDiscountService offerDiscountService)
    {
        _offerDiscountService = offerDiscountService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _offerDiscountService.GetAllAsync(HttpContext.RequestAborted));
    }
}
