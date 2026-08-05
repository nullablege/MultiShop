using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents;

public sealed class DailyOfferViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
