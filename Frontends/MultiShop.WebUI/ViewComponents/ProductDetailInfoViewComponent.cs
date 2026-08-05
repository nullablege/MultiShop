using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents;

public sealed class ProductDetailInfoViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
