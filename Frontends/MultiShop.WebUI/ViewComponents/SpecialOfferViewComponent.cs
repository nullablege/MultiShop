using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents;

public sealed class SpecialOfferViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
