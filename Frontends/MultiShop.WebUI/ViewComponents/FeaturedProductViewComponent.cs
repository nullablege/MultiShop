using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents;

public sealed class FeaturedProductViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
