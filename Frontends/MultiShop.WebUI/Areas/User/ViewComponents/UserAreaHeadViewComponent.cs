using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.Areas.User.ViewComponents;

public sealed class UserAreaHeadViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
