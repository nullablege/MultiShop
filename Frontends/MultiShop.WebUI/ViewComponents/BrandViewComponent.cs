using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents;

public sealed class BrandViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
