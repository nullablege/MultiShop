using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents;

public sealed class PaymentMethodViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
