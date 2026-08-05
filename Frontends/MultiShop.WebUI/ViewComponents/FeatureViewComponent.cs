using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents;

public sealed class FeatureViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
