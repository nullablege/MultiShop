using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents;

public sealed class ProductReviewViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
