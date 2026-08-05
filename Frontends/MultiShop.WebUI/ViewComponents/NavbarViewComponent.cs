using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents;

public sealed class NavbarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
