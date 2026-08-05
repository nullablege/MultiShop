using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents;

public sealed class CarouselViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
