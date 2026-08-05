using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents;

public sealed class TopbarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
